using AutoMapper;
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

        public AuthService(UserManager<User> userManager,RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt,
            IMapper mapper, IOptions<TwilioSetting> twilio, IHttpContextAccessor httpContextAccessor, IStringLocalizer<AuthService> localization)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _twilio = twilio.Value;
            _localization = localization;
        }

        

        public async Task<string> RegisterAsync(RiderRegisterDto Input)
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
          //  var roleEx = Checkrole(Input.Role);
            if (result.Succeeded)
            {
                if (/*!roleEx*/ Input.Role==null)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Role_Rider);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, Input.Role);
                }
                return _localization["RegisterSucccess"].Value;

            }
            else
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description},";
                throw new BadRequestException(errors);
            }
        }
        public async Task<MessageResource> SendMessage(PhoneDto phoneNumber)
        {
            string otp = await GenerateOtp();
            SetOtpInCookie(otp);
            TwilioClient.Init(_twilio.AccountSID, _twilio.AuthToken);
            var message = await MessageResource.CreateAsync(
                body: $"Your Code is: {otp}",
                from: new Twilio.Types.PhoneNumber(_twilio.TwilioPhoneNumber),
                  to: new Twilio.Types.PhoneNumber(phoneNumber.Phone)

                );
            return message;
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
        public async Task<bool> CompareOtp(string reciveOtp)
        {
           // var user = await getUserAsync(phone);
            var otp = _httpContextAccessor.HttpContext.Request.Cookies["storeOtp"];
            if (otp == reciveOtp)
             {
                return true;
             }
            throw new BadRequestException(_localization["compareOtp"].Value);
        }
        public async Task<userDto> ConfirmPhone(ConfirmNumberDto confirmNumberDto)
        {
            var user = await getUserAsync(confirmNumberDto.Phone);

            bool checkotp = await CompareOtp(confirmNumberDto.RecOtp) ;
            if(!checkotp)
            {
                throw new BadRequestException(_localization["ConfirmPhone"].Value);
            }
            user.PhoneNumberConfirmed = true;
            var jwtSecurityToken = await CreateToken(user);
              var refreshToken = GenerateRefreshToken();
             user.RefreshTokens?.Add(refreshToken);
              await _userManager.UpdateAsync(user);
             var roles = await _userManager.GetRolesAsync(user);

            return new userDto
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                isAuthenticated = true,
                Role = roles[0],
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
                phoneNumber=user.PhoneNumber,
                RefreshToken = refreshToken.RefToken,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };
        }
        public async Task<string> ResetPassword(ResetPasswordDto resetPassword)
        {
            var user = await getUserAsync(resetPassword.Phone);
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPassword.newPassword);
            if (!result.Succeeded)
                throw new BadRequestException(_localization["ResetPassword"].Value);
            return _localization["PasswordChanged"].Value;                
        }
        public async Task<userDto> RefreshTokenAsync(RefTokenDto token)
        {
            var userdto = new userDto();
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefToken == token.refToken));
          
          var revoke=await RevokeTokenAsync(token);
            if (revoke)
            {
                var newRefreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);

                var jwtSecurityToken = await CreateToken(user);
                var roles = await _userManager.GetRolesAsync(user);
                userdto.Email = user.Email;
                userdto.UserName = user.UserName;
                userdto.phoneNumber = user.PhoneNumber;
                userdto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                userdto.isAuthenticated = true;
                userdto.ExpiresOn = jwtSecurityToken.ValidTo;
                userdto.Role = roles[0];
                userdto.RefreshToken = newRefreshToken.RefToken;
                userdto.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            }
            else
            {
                throw new BadHttpRequestException(_localization["generateRefreshToken"].Value);
            }
            return userdto;
        }
        public async Task<string> LogoutAsync(RefTokenDto token)
        {
          var resaul=await this.RevokeTokenAsync(token);
            if (!resaul)
              throw new BadRequestException(_localization["logoutWrong"].Value);
            return _localization["LogoutSuccess"].Value;
        }
        private async Task<bool> RevokeTokenAsync(RefTokenDto token)
        {
            var userdto = new userDto();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefToken == token.refToken));

            if (user == null)
            {
                throw new NotFoundException(_localization["refreshTokenNotFound"].Value);
            }
            var refreshToken = user.RefreshTokens.Single(t => t.RefToken == token.refToken);

            if (!refreshToken.IsActive)
            {
                throw new UnauthorizedException(_localization["Unauthorized"].Value);
            }
            user.RefreshTokens.Remove(refreshToken);
            await _userManager.UpdateAsync(user);
            return true;
        }
        public async Task<userDto> LoginAsync(RiderLoginDto Input)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == Input.PhoneNumber);
            if (user is  null|| !await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                throw new BadRequestException(_localization["phonOrpasswordNotCorrect"].Value);

            }
            var jwtSecurityToken = await CreateToken(user);
            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens?.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new userDto
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                isAuthenticated = true,
                Role = roles[0],
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
                phoneNumber = user.PhoneNumber,
                RefreshToken = refreshToken.RefToken,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };

        }
        private async Task<User> getUserAsync(string phoneNumber)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
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
                Expires = DateTime.Now.AddMinutes(10),
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
