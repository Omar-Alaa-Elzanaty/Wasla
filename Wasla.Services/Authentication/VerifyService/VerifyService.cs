﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.EmailServices;
using Wasla.Services.Exceptions;
using Wasla.Services.MediaSerivces;

namespace Wasla.Services.Authentication.VerifyService
{
    public class VerifyService : IVerifyService
    {
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TwilioSetting _twilio;
        private readonly JWT _jwt;
        private readonly IStringLocalizer<VerifyService> _localization;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseResponse _response;
        private readonly WaslaDb _dbContext;
        private readonly IMailServices _mailService;

        public VerifyService
            (
            UserManager<Account> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<JWT> jwt,
            IOptions<TwilioSetting> twilio,
            IStringLocalizer<VerifyService> localization,
            WaslaDb dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _twilio = twilio.Value;
            _localization = localization;
            _response = new();
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse> SendOtpMessageAsync(string userPhone)
        {
            string otp = await GenerateOtp();
            SetOtpInCookie(otp);
            await SendMessage(userPhone, otp);
            _response.Message = _localization["SendMsgSuccess"].Value;
            _response.Data = otp;
            return _response;
        }

        public async Task<BaseResponse> SendOtpEmailAsync(string userEmail)
        {
            string otp = await GenerateOtp();
            SetOtpInCookie(otp);
            await _mailService.SendEmailAsync(
                             mailTo: userEmail,
                             subject: "Your OTP",
                             body: "Your OTP is: " + otp);

            _response.Message = _localization["EmailSendSuccess"].Value;
            _response.Data = otp;
            return _response;
        }
        public async Task<BaseResponse> CheckUserNameSimilarity(string input)
        {
            if (input.IsNullOrEmpty())
            {
                throw new BadRequestException(_localization["userNameRequired"].Value);
            }

            bool isNotFound = !await _userManager.Users.AnyAsync(a => a.UserName != null && a.UserName.StartsWith(input));

            _response.Data = new
            {
                Valid = isNotFound,
                Message = isNotFound ? _localization["NotUsed"].Value : _localization["Used"].Value
            };

            return _response;
        }
        public async Task<BaseResponse> CheckPhoneNumberAsync(string phoneNumber)
        {
            if (phoneNumber.IsNullOrEmpty())
            {
                throw new BadRequestException(_localization["phoneNumberRequired"]);
            }

            var isNotFound = !await _userManager.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);

            _response.Data = new
            {
                Valid = isNotFound,
                Message = isNotFound ? _localization["NotUsed"].Value : _localization["Used"].Value
            };
            return _response;
        }
        public async Task<BaseResponse> CheckEmailAsync(string email)
        {
            if (email.IsNullOrEmpty())
            {
                throw new BadRequestException(_localization["EmailRequired"]);
            }

            var isNotFound = !await _userManager.Users.AnyAsync(u => u.Email == email);

            _response.Data = new
            {
                Valid = isNotFound,
                Message = isNotFound ? _localization["NotUsed"].Value : _localization["Used"].Value
            };
            return _response;
        }

        public async Task<BaseResponse> ChangePasswordAsync(string token, ChangePasswordDto changePassword)
        {
            var user = await getUserByToken(token);
            if (!await _userManager.CheckPasswordAsync(user, changePassword.OldPassword))
            {
                throw new BadRequestException(_localization["userOrpasswordNotCorrect"].Value);
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, changePassword.NewPassword);
            if (!result.Succeeded)
                throw new BadRequestException(_localization["ResetPassword"].Value);

            _response.Message = _localization["PasswordChanged"].Value;
            return _response;
        }
        public async Task<BaseResponse> ConfirmPhoneAsync(ConfirmNumberDto confirmNumberDto)
        {
            var user = await getUserByPhone(confirmNumberDto.Phone);

            bool checkotp = CheckOtp(confirmNumberDto.RecOtp);
            if (!checkotp)
            {
                throw new BadRequestException(_localization["ConfirmPhone"].Value);
            }
            user.PhoneNumberConfirmed = true;
            await _userManager.UpdateAsync(user);
            _response.Message = _localization["PhoneConfirmedSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
        {
            var user = await getUserByEmail(confirmEmailDto.Email);

            bool checkotp = CheckOtp(confirmEmailDto.RecOtp);
            if (!checkotp)
            {
                throw new BadRequestException(_localization["ConfirmEmailError"].Value);
            }
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            _response.Message = _localization["EmailConfirmSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> CompareOtpAsync(string otp)
        {
            var res = CheckOtp(otp);
            _response.Data = otp;
            _response.Message = _localization["otpSame"].Value;
            return _response;
        }
        public async Task<BaseResponse> EditEmailAsync(string RefreshToken, string newEmail)
        {
             var user=await getUserByToken(RefreshToken);
              await CheckEmail(newEmail);
              user.Email=newEmail;
            await _userManager.UpdateAsync(user);
            _response.Message = _localization["EmailEditSuccess"].Value;
            return _response;
        }

        public async Task<BaseResponse> EditPhoneAsync(string RefreshToken, string newPhone)
        {
            var user = await getUserByToken(RefreshToken);
            await CheckPhoneNumber(newPhone);
            user.PhoneNumber = newPhone;
            await _userManager.UpdateAsync(user);
            _response.Message = _localization["PhoneEditSuccess"].Value;
            return _response;
        }
        private async Task<Account> getUserByToken(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefToken == token));
            if (user == null)
                throw new NotFoundException(_localization["refreshTokenNotFound"].Value);
            return user;
        }
        private async Task<Account> getUserByPhone(string phoneNumber)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user == null)
                throw new NotFoundException(_localization["PhoneNumberWrong"].Value);
            return user;
        }
        private async Task<Account> getUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException(_localization["EmailNotFound"].Value);
            return user;
        }
        private async Task<bool> CheckPhoneNumber(string PhoneNumber)
        {
            if (await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == PhoneNumber) is not null)
            {
                throw new BadRequestException(_localization["phoneNumberExist"].Value);
            }
            return false;
        }
        private async Task<bool> CheckEmail(string Email)
        {
            if (await _userManager.FindByEmailAsync(Email) is not null)
            {
                throw new BadRequestException(_localization["EmailExist"].Value);
            }
            return false;
        }
        private void SetOtpInCookie(string otp)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(60),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("storeOtp", otp, cookieOptions);
        }
        private async Task<bool> SendMessage(string sendOtpDto, string msg)
        {
            try
            {
                TwilioClient.Init(_twilio.AccountSID, _twilio.AuthToken);
                var message = await MessageResource.CreateAsync(
                    body: $"Your Code is: {msg}",
                    from: new Twilio.Types.PhoneNumber(_twilio.TwilioPhoneNumber),
                      to: new Twilio.Types.PhoneNumber(sendOtpDto)
                    );
                return true;
            }
            catch (Exception)
            {
                throw new BadHttpRequestException(_localization["errorInSendMsg"].Value);
            }
        }
        private async Task<string> GenerateOtp()
        {
            var random = new Random();
            var otp = random.Next(100000, 999999).ToString();
            return otp;
        }
        private bool CheckOtp(string reciveOtp)
        {
            var otp = _httpContextAccessor.HttpContext.Request.Cookies["storeOtp"];
            if (otp != reciveOtp)
            {
                throw new BadRequestException(_localization["otpWrong"].Value);
            }
            return true;
        }

    }
}
