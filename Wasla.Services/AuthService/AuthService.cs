﻿using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.Exceptions;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Localization;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace Wasla.Services.AuthService
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TwilioSetting _twilio;
        private readonly JWT _jwt;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AuthService> _localization;
        private readonly BaseResponse _response;
        private readonly SmtpSettings _smtpSettings;


        public AuthService(UserManager<User> userManager,RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt,
            IMapper mapper, IOptions<TwilioSetting> twilio, IHttpContextAccessor httpContextAccessor, IOptions<SmtpSettings> smtpSettings,
            IStringLocalizer<AuthService> localization)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _twilio = twilio.Value;
            _localization = localization;
            _response = new();
            _smtpSettings = smtpSettings.Value;


        }

        public async Task<BaseResponse> RegisterAsync(PassengerRegisterDto Input)
        {
            
            if (await _userManager.Users.FirstOrDefaultAsync(u=>u.PhoneNumber==Input.PhoneNumber) is not null)
            {
                throw new BadRequestException(_localization["phoneNumberExist"].Value);
            }
            if (await _userManager.FindByNameAsync(Input.UserName) is not null)
            {
                throw new BadRequestException(_localization["userNameExist"].Value);

            }
            if ( Input.Email is not null &&await _userManager.FindByEmailAsync(Input.Email) is not null)
            {
                throw new BadRequestException(_localization["EmailExist"].Value);

            }

            var user=_mapper.Map<Customer>(Input);
            var result = await _userManager.CreateAsync(user, Input.Password);
            var role = Roles.Role_Rider;
            //  var roleEx = Checkrole(Input.Role);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description},";
                throw new BadRequestException(errors);
            }
                if (Input.Role==null)
                {
                    await _userManager.AddToRoleAsync(user,role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, Input.Role);
                role = Input.Role;
                }
            var passengerDto = new PassengerResponseDto();
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            var jwtSecurityToken = await CreateToken(user);
            passengerDto.ConnectionData.Email = Input.Email;
            passengerDto.UserName = Input.UserName;
            passengerDto.ConnectionData.phone = Input.PhoneNumber;
            passengerDto.TokensData.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            passengerDto.IsAuthenticated = true;
            passengerDto.TokensData.TokenExpiryDate = jwtSecurityToken.ValidTo;
            passengerDto.Role = role;
            passengerDto.TokensData.RefreshToken = newRefreshToken.RefToken;
            passengerDto.TokensData.RefTokenExpiryDate = newRefreshToken.ExpiresOn;
            _response.Message = _localization["RegisterSucccess"].Value;
            _response.Data = passengerDto;
                return _response;

        }
        public async Task<BaseResponse>SendOtpMessageAsync(string userPhone)
        {
            string otp = await GenerateOtp();
            SetOtpInCookie(otp);
            await this.SendMessage(userPhone, otp);
            _response.Message = _localization["SendMsgSuccess"].Value;
            _response.Data = otp;
            return _response;
        }
        
       public async Task<BaseResponse> SendOtpEmailAsync(string userEmail)
        {
            string otp = await GenerateOtp();
            SetOtpInCookie(otp);
             await this.SendEmail(userEmail, otp);
            _response.Message = _localization["EmailSendSuccess"].Value;
            _response.Data = otp;
            return _response;
        }     
        public async Task<BaseResponse> ConfirmPhoneAsync(ConfirmNumberDto confirmNumberDto)
        {
            var user = await getUserByPhone(confirmNumberDto.Phone);

            bool checkotp =  CheckOtp(confirmNumberDto.RecOtp) ;
            if(!checkotp)
            {
                throw new BadRequestException(_localization["ConfirmPhone"].Value);
            }
            user.PhoneNumberConfirmed = true;
            _response.Message= _localization["PhoneConfirmedSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
        {
            var user = await getUserByEmail(confirmEmailDto.Email);

            bool checkotp = CheckOtp(confirmEmailDto.RecOtp);
            if (!checkotp)
            {
                throw new BadRequestException(_localization["ConfirmPhone"].Value);
            }
            user.EmailConfirmed = true;
            _response.Message = _localization["EmailConfirmSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> ResetPasswordByphoneAsync(ResetPasswordDto resetPassword)
        {
            var user = await getUserByPhone(resetPassword.Contact);
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPassword.NewPassword);
            if (!result.Succeeded)
                throw new BadRequestException(_localization["ResetPassword"].Value);
            _response.Message= _localization["PasswordChanged"].Value;
            return _response;             
        }
        public async Task<BaseResponse> ResetPasswordByEmailAsync(ResetPasswordDto resetPassword)
        {
            var user = await getUserByEmail(resetPassword.Contact);
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPassword.NewPassword);
            if (!result.Succeeded)
                throw new BadRequestException(_localization["ResetPassword"].Value);
            _response.Message = _localization["PasswordChanged"].Value;
            return _response;
        }
        public async Task<BaseResponse> RefreshTokenAsync(RefTokenDto token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefToken == token.RefToken));
          
          var revoke=await RevokeTokenAsync(token);
            if (!revoke)
            {
                throw new BadHttpRequestException(_localization["generateRefreshToken"].Value);
            }
                var passengerDto = new PassengerResponseDto();
                var newRefreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);
                var jwtSecurityToken = await CreateToken(user);
                var roles = await _userManager.GetRolesAsync(user);
            passengerDto.ConnectionData.Email = user.Email;
            passengerDto.UserName = user.UserName;
            passengerDto.ConnectionData.phone = user.PhoneNumber;
            passengerDto.TokensData.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            passengerDto.IsAuthenticated = true;
            passengerDto.TokensData.TokenExpiryDate = jwtSecurityToken.ValidTo;
            passengerDto.Role = roles[0];
            passengerDto.TokensData.RefreshToken = newRefreshToken.RefToken;
            passengerDto.TokensData.RefTokenExpiryDate = newRefreshToken.ExpiresOn;
            _response.Message = _localization["refreshTokenCreatedSuccess"].Value;
            _response.Data = passengerDto;
            return _response;
        }
        public async Task<BaseResponse> LogoutAsync(RefTokenDto token)
        {
          var resaul=await this.RevokeTokenAsync(token);
            if (!resaul)
              throw new BadRequestException(_localization["logoutWrong"].Value);
            _response.Message = _localization["LogoutSuccess"].Value;
            return _response;
        }
      public async Task<BaseResponse> CompareOtpAsync(string otp)
        {
            var res = CheckOtp(otp);
            _response.Data = otp;
            _response.Message = _localization["otpSame"].Value;
            return _response;
        }
        public async Task<BaseResponse> LoginAsync(LoginDto Input)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == Input.Phone);
            if (user is  null|| !await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                throw new BadRequestException(_localization["phonOrpasswordNotCorrect"].Value);

            }
            var passengerDto = new PassengerResponseDto();
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            var jwtSecurityToken = await CreateToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            passengerDto.ConnectionData.Email = user.Email;
            passengerDto.UserName = user.UserName;
            passengerDto.ConnectionData.phone = user.PhoneNumber;
            passengerDto.TokensData.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            passengerDto.IsAuthenticated = true;
            passengerDto.TokensData.TokenExpiryDate = jwtSecurityToken.ValidTo;
            passengerDto.Role = roles[0];
            passengerDto.TokensData.RefreshToken = newRefreshToken.RefToken;
            passengerDto.TokensData.RefTokenExpiryDate = newRefreshToken.ExpiresOn;
            _response.Message= _localization["LoginSuccess"].Value;
            _response.Data = passengerDto;
            return _response;
            
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
            catch (Exception )
            {
                throw new BadHttpRequestException(_localization["errorInSendMsg"].Value);
            }
        }
        private async Task<bool> SendEmail(string userEmail, string mail)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_smtpSettings.Sender));
                email.To.Add(MailboxAddress.Parse(userEmail));
                email.Subject = "Your OTP";
                email.Body = new TextPart(TextFormat.Plain) { Text = "Your OTP is: " + mail };
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_smtpSettings.Sender, _smtpSettings.Port);
                await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await smtp.SendAsync(email);
                 await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception)
            {
                throw new BadHttpRequestException(_localization["errorInSendEmail"].Value);
            }

        }
        private  bool CheckOtp(string reciveOtp)
        {
            // var user = await getUserAsync(phone);
            var otp = _httpContextAccessor.HttpContext.Request.Cookies["storeOtp"];
            if (otp != reciveOtp)
            {
                throw new BadRequestException(_localization["compareOtp"].Value);
            }
            return true;
        }
        private async Task<string> GenerateOtp()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var otp = new string(stringChars);
            return otp;

        }
        private async Task<User> getUserByPhone(string phoneNumber)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user == null)
                throw new NotFoundException(_localization["UserNotFound"].Value);
            return user;
        }
        private async Task<User> getUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException(_localization["UserNotFound"].Value);
            return user;
        }
        private async Task<JwtSecurityToken> CreateToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
        private async Task<bool> RevokeTokenAsync(RefTokenDto token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefToken == token.RefToken));

            if (user == null)
            {
                throw new NotFoundException(_localization["refreshTokenNotFound"].Value);
            }
            var refreshToken = user.RefreshTokens.Single(t => t.RefToken == token.RefToken);

            if (!refreshToken.IsActive)
            {
                throw new UnauthorizedException(_localization["Unauthorized"].Value);
            }
            user.RefreshTokens.Remove(refreshToken);
            await _userManager.UpdateAsync(user);
            return true;
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                RefToken = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(20),
                CreatedOn = DateTime.UtcNow
            };
        }
        private void SetOtpInCookie(string otp)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(30),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("storeOtp", otp, cookieOptions);
        }
        private bool Checkrole(string roleName)
        {
            var res= _roleManager.RoleExistsAsync(roleName);
            return res!=null;

        }
    }
}
