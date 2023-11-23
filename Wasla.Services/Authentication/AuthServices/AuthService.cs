using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.Exceptions;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Localization;
using Wasla.DataAccess;
using Wasla.Services.MediaSerivces;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory;
using System.Data;
using Wasla.Services.EmailServices;

namespace Wasla.Services.Authentication.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TwilioSetting _twilio;
        private readonly JWT _jwt;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AuthService> _localization;
        private readonly BaseResponse _response;
        private readonly SmtpSettings _smtpSettings;
        private readonly IBaseFactoryResponse _baseFactory;
        private readonly WaslaDb _dbContext;
        private readonly IMediaSerivce _mediaServices;
        private readonly ILogger<AuthService> _logger;
        private readonly IMailServices _mailService;

        public AuthService
            (
            IBaseFactoryResponse baseFactory,
            UserManager<Account> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWT> jwt,
            IMapper mapper,
            IOptions<TwilioSetting> twilio,
            IHttpContextAccessor httpContextAccessor,
            IOptions<SmtpSettings> smtpSettings,
            IStringLocalizer<AuthService> localization,
            IMediaSerivce mediaSerivces,
            WaslaDb dbContext,
            ILogger<AuthService> logger)
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
            _dbContext = dbContext;
            _mediaServices = mediaSerivces;
            _logger = logger;
            _mediaServices = mediaSerivces;
            _baseFactory = baseFactory;
        }

        private void CheckPhoneNumber(string PhoneNumber)
        {
            if (await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == PhoneNumber) is not null)
            {
                throw new BadRequestException(_localization["phoneNumberExist"].Value);
            }
        }
        private void CheckUserName(string UserName)
        {
            if (await _userManager.FindByNameAsync(UserName) is not null)
            {
                throw new BadRequestException(_localization["userNameExist"].Value);
            }
        }
        private void CheckEmail(string Email)
        {
            if (await _userManager.FindByNameAsync(Email) is not null)
            {
                throw new BadRequestException(_localization["userNameExist"].Value);
            }
        }

        public async Task<BaseResponse> RegisterPassengerAsync(PassengerRegisterDto Input)
        {
            CheckPhoneNumber(Input.PhoneNumber);
            CheckUserName(Input.UserName);
            CheckEmail(Input.Email);
            var user = _mapper.Map<Customer>(Input);
            var result = await _userManager.CreateAsync(user, Input.Password);
            var role = Roles.Role_Rider;
            if (!result.Succeeded)
            {
                throw new BadRequestException(_localization["RegisterFaild"].Value);
            }
            await _userManager.AddToRoleAsync(user, role);

            var tokens = await GetTokenhelp(user, role);
            var response = await _baseFactory.BaseAuthResponseAsync(tokens);
            response.Message = _localization["RegisterSucccess"].Value;

            return response;
        }

        public async Task<BaseResponse> OrgnaizationRegisterAsync(OrgRegisterRequestDto request)
        {

            _ = CheckOtp(request.Otp);

            if (await _dbContext.OrganizationsRegisters.AnyAsync(o => o.Email == request.Email)
                || await _userManager.FindByIdAsync(request.Email) is not null)
            {
                throw new BadRequestException(_localization["EmailExist"].Value);
            }
            if (await _dbContext.OrganizationsRegisters.AnyAsync(o => o.Name == request.Name)
              || await _dbContext.Organizations.AnyAsync(o => o.Name == request.Name))
            {
                throw new BadRequestException(_localization["OrganizationNameExist"].Value);
            }

            if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber)
                || await _dbContext.OrganizationsRegisters.AnyAsync(o => o.PhoneNumber == request.PhoneNumber))
            {
                throw new BadRequestException(_localization["phoneNumberExist"]);
            }

            OrganizationRegisterRequest orgRequest = _mapper.Map<OrganizationRegisterRequest>(request);


            orgRequest.ImageUrl = await _mediaServices.AddAsync(request.ImageFile);

            _ = await _dbContext.OrganizationsRegisters.AddAsync(orgRequest);
            _ = _dbContext.SaveChanges();

            _response.Message = _localization["OrganizationSuccessRequest"];
            return _response;
        }

        public async Task<BaseResponse> DriverRegisterAsync(DriverRegisterDto model)
        {
            CheckPhoneNumber(model.PhoneNumber);
            CheckEmail(model.Email);

            if (await _dbContext.Drivers.AnyAsync(u => u.LicenseNum == model.LicenseNum))
            {
                throw new BadRequestException(_localization["LicenseExist"]);
            }

            var user = _mapper.Map<Driver>(model);
            user.LicenseImageUrl = await _mediaServices.AddAsync(model.LicenseImageFile);
            user.PhotoUrl = await _mediaServices.AddAsync(model.ProfileImageFile);
            var role = Roles.Role_Driver;
          /*  var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);*/

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                var x = await _userManager.CreateAsync(user, model.Password);
                var result = await _userManager.AddToRoleAsync(user,role);
                if (!result.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";

                    await transaction.RollbackAsync();
                    throw new BadRequestException(errors);
                }
                await transaction.CommitAsync();
            }
            var tokens = await GetTokenhelp(user,role);
            var response = await _baseFactory.BaseAuthResponseAsync(tokens);
            response.Message = _localization["RegisterSucccess"].Value;
            return response;
            /*  var jwtSecurityToken = await CreateToken(user);
              var driverResponse = new DataAuthResponse();
              driverResponse.ConnectionData.Email = user.Email;
              driverResponse.UserName = user.UserName;
              driverResponse.ConnectionData.phone = user.PhoneNumber;
              driverResponse.TokensData.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
              driverResponse.IsAuthenticated = true;
              driverResponse.TokensData.TokenExpiryDate = jwtSecurityToken.ValidTo;
              driverResponse.Role = Roles.Role_Driver;
              driverResponse.TokensData.RefreshToken = newRefreshToken.RefToken;
              driverResponse.TokensData.RefTokenExpiryDate = newRefreshToken.ExpiresOn;

              _response.Message = _localization["RegisterSucccess"].Value;
              _response.Data = driverResponse;

              return _response;*/
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
        public async Task<BaseResponse> ResetPasswordByphoneAsync(ResetPasswordDto resetPassword)
        {
            var user = await getUserByPhone(resetPassword.UserName);
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPassword.NewPassword);
            if (!result.Succeeded)
                throw new BadRequestException(_localization["ResetPassword"].Value);
            _response.Message = _localization["PasswordChanged"].Value;
            return _response;
        }
        public async Task<BaseResponse> ResetPasswordByEmailAsync(ResetPasswordDto resetPassword)
        {
            var user = await getUserByEmail(resetPassword.UserName);
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

            var revoke = await RevokeTokenAsync(token);
            if (!revoke)
            {
                throw new BadHttpRequestException(_localization["generateRefreshToken"].Value);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var tokens = await GetTokenhelp(user, roles[0]);
            var response = await _baseFactory.BaseAuthResponseAsync(tokens);
            response.Message = _localization["refreshTokenCreatedSuccess"].Value;
            return response;
        }
        public async Task<BaseResponse> LogoutAsync(RefTokenDto token)
        {
            var resaul = await RevokeTokenAsync(token);
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
        public async Task<BaseResponse> LoginAsync(LoginDto input)
        {
            var org = input.role == Roles.Role_Organization;
            var checkedUser = await CheckUser(input,org);
            if (checkedUser.role != input.role)
                throw new BadRequestException(_localization["roleNotMatch"].Value);
            var tokens = await GetTokenhelp(checkedUser.user, input.role);
            var response = await _baseFactory.BaseAuthResponseAsync(tokens);
            response.Message = _localization["LoginSuccess"].Value;
            return response;
        }
        public async Task<BaseResponse> CheckUserNameSimilarity(string input)
        {
            if (input.IsNullOrEmpty())
            {
                throw new BadRequestException(_localization["userNameRequired"].Value);
            }

            bool isFound = await _userManager.Users.AnyAsync(a => a.UserName != null && a.UserName.StartsWith(input));

            if (!isFound)
            {
                _response.Message = _localization["userNameExist"].Value;
            }

            return _response;
        }
        public async Task<BaseResponse>CheckPhoneNumberAsync(string phoneNumber)
        {
            if(phoneNumber.IsNullOrEmpty())
            {
                throw new BadRequestException(_localization["phoneNumberRequired"]);
            }

            _response.IsSuccess = await _userManager.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
            _response.Status =
                _response.IsSuccess == true ? HttpStatusCode.BadRequest : HttpStatusCode.OK;
			_response.Message =
                _response.IsSuccess == true ? _localization["phoneNumberExist"].Value : _localization["PhoneNumberValid"].Value;

            return _response;
        }
		public async Task<BaseResponse> CheckEmailAsync(string email)
		{
			if (email.IsNullOrEmpty())
			{
				throw new BadRequestException(_localization["EmailRequired"]);
			}

			_response.IsSuccess = await _userManager.Users.AnyAsync(u => u.Email == email);
			_response.Status =
				_response.IsSuccess == true ? HttpStatusCode.BadRequest : HttpStatusCode.OK;
			_response.Message =
				_response.IsSuccess == true ? _localization["EmailExist"].Value : _localization["EmailValid"].Value;

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
            catch (Exception)
            {
                throw new BadHttpRequestException(_localization["errorInSendMsg"].Value);
            }
        }
        private bool CheckOtp(string reciveOtp)
        {
            // var user = await getUserAsync(phone);
            var otp = _httpContextAccessor.HttpContext.Request.Cookies["storeOtp"];
            if (otp != reciveOtp)
            {
                throw new BadRequestException(_localization["otpWrong"].Value);
            }
            return true;
        }
       
        private async Task<string> GenerateOtp()
        {
            var random = new Random();
            var otp = random.Next(100000, 999999).ToString();
            return otp;
        }
        private async Task<Account> getUserByPhone(string phoneNumber)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user == null)
                throw new NotFoundException(_localization["PhoneNumberWrong"].Value);
            return user;
        }
        private async Task<CheckUserExit> CheckUser(LoginDto login,bool org=false)
        {
            const string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            const string phonePattern = @"^(?:\+?20)?(?:0)?1[0-2]\d{8}$";

            Account user = null;
            if (Regex.IsMatch(login.UserName, emailPattern))
            {
                if (org)
                    user = await checkEmailExitInOrgOrOrgRegister(login.UserName);
                  user = await getUserByEmail(login.UserName);
            }
            else if (Regex.IsMatch(login.UserName, phonePattern))
            {
                user = await getUserByPhone(login.UserName);
            }
            else
            {
                if(org)
                throw new NotFoundException(_localization["UserNameNotFound"].Value);
                user = await getByUserName(login.UserName);
            }
            if (!await _userManager.CheckPasswordAsync(user, login.Password))
            {
                throw new BadRequestException(_localization["userOrpasswordNotCorrect"].Value);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var checkUser = new CheckUserExit
            {
                role = roles.FirstOrDefault(),
                user = user
            };
            return checkUser;
        }
        private async Task<AuthResponseFactoryHelp> GetTokenhelp(Account user, string role)
        {
            var newRefreshToken = GenerateRefreshToken();
            var jwtSecurityToken = await CreateToken(user);
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            var authHelp = new AuthResponseFactoryHelp();
            authHelp.TokensData.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authHelp.TokensData.TokenExpiryDate = jwtSecurityToken.ValidTo;
            authHelp.TokensData.RefreshToken = newRefreshToken.RefToken;
            authHelp.TokensData.RefTokenExpiryDate = newRefreshToken.ExpiresOn;
            authHelp.role = role;
            authHelp.userId = user.Id;
            return authHelp;
        }
        private async Task<Account> getByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new NotFoundException(_localization["UserNameNotFound"].Value);
            return user;
        }
        private async Task<Account> getUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException(_localization["EmailNotFound"].Value);
            return user;
        }
        private async Task<Account> checkEmailExitInOrgOrOrgRegister(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                return user;
            if (await _dbContext.OrganizationsRegisters.AnyAsync(o => o.Email == email))
                throw new BadHttpRequestException(_localization["EmailInOrgRegister"].Value);
            throw new NotFoundException(_localization["EmailNotFound"].Value);
        }
        private async Task<JwtSecurityToken> CreateToken(Account account)
        {
            var userClaims = await _userManager.GetClaimsAsync(account);
            var roles = await _userManager.GetRolesAsync(account);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim("uid", account.Id)
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
                Expires = DateTime.Now.AddMinutes(60),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("storeOtp", otp, cookieOptions);
        }
    }
}
