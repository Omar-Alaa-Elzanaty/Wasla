using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Statics;
using Wasla.Model.Models;
using Wasla.Services.Exceptions;
using Wasla.Services.ShareService.AuthVerifyShareService;
using Wasla.Services.ShareService.EmailServices;

namespace Wasla.Services.Authentication.VerifyService
{
    public class VerifyService : IVerifyService
    {
        private readonly UserManager<Account> _userManager;
        private readonly TwilioSetting _twilio;
        private readonly IStringLocalizer<VerifyService> _localization;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseResponse _response;
        private readonly IMailServices _mailService;
        private readonly IAuthVerifyService _authVerifyService;
        public VerifyService
        (
            UserManager<Account> userManager, 
            IOptions<TwilioSetting> twilio,
            IStringLocalizer<VerifyService> localization,
            IAuthVerifyService authVerifyService
            , IHttpContextAccessor httpContextAccessor,IMailServices mailServices)
        {
            _userManager = userManager;
            _twilio = twilio.Value;
            _localization = localization;
            _authVerifyService = authVerifyService;
            _response = new();
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailServices;
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
            if (_mailService != null)
            {

                await _mailService.SendEmailAsync(
                                mailTo: userEmail,
                                subject: "Your OTP",
                                body: "Your OTP is: " + otp);

                _response.Message = _localization["EmailSendSuccess"].Value;
                _response.Data = otp;
            }
            else
                _response.Message = "mail Service is null";
             return _response;
            
        }
        public async Task<BaseResponse> CheckUserNameSimilarity(string input)
        {
            if (input.IsNullOrEmpty())
            {
               return  BaseResponse.GetErrorException(HttpStatusErrorCode.BadRequest, (_localization["userNameRequired"].Value));
             //   throw new BadRequestException(_localization["userNameRequired"].Value);
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

        public async Task<BaseResponse> ChangePasswordAsync(ChangePasswordDto changePassword)
        {
            var user = await _authVerifyService.getUserByToken(changePassword.Reftoken);
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
            var user = await _authVerifyService.getUserByPhone(confirmNumberDto.Phone);

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
            var user = await _authVerifyService.getUserByEmail(confirmEmailDto.Email);

            bool checkotp = CheckOtp(confirmEmailDto.RecOtp);
            if (!checkotp)
            {
               //await _errorException.GetError(400, _localization["ConfirmEmailError"].Value);
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
        public async Task<BaseResponse> EditEmailAsync(EditEmailDto email)
        {
             var user=await _authVerifyService.getUserByToken(email.Reftoken);
              await _authVerifyService.CheckEmail(email.Email);
              user.Email=email.Email;
            user.NormalizedEmail = email.Email.ToUpper();
            await _userManager.UpdateAsync(user);
            _response.Message = _localization["EmailEditSuccess"].Value;
            return _response;
        }

        public async Task<BaseResponse> EditPhoneAsync(EditPhoneDto phone)
        {
            var user = await _authVerifyService.getUserByToken(phone.Reftoken);
            await _authVerifyService.CheckPhoneNumber(phone.Phone);
            user.PhoneNumber = phone.Phone;
            await _userManager.UpdateAsync(user);
            _response.Message = _localization["PhoneEditSuccess"].Value;
            return _response;
        }
     public async Task<string> SetOtp()
        {
            string otp = await GenerateOtp();
            SetOtpInCookie(otp);
            return otp;
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
            var otp = random.Next(1000, 9999).ToString();
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
