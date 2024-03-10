using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Wasla.Model.Models;
using Wasla.Services.Authentication.AuthServices;
using Wasla.Services.Exceptions;

namespace Wasla.Services.ShareService.AuthVerifyShareService
{
    public class AuthVerifyService:IAuthVerifyService
    {
        private readonly UserManager<Account> _userManager;
        private readonly IStringLocalizer<AuthService> _localization;

        public AuthVerifyService(UserManager<Account> userManager, IStringLocalizer<AuthService> localization)
        {
            _userManager = userManager;
            _localization = localization;
        }

        public async Task<Account> getUserByToken(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefToken == token));
            if (user == null)
                throw new NotFoundException(_localization["refreshTokenNotFound"].Value);
            return user;
        }
        public async Task<Account> getUserByPhone(string phoneNumber)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user == null)
                throw new NotFoundException(_localization["PhoneNumberWrong"].Value);
            return user;
        }
        public async Task<Account> getUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException(_localization["EmailNotFound"].Value);
            return user;
        }
        public async Task<bool> CheckPhoneNumber(string PhoneNumber)
        {
            if (await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == PhoneNumber) is not null)
            {
                throw new BadRequestException(_localization["phoneNumberExist"].Value);
            }
            return false;
        }
        public async Task<bool> CheckEmail(string Email)
        {
            if (await _userManager.FindByEmailAsync(Email) is not null)
            {
                throw new BadRequestException(_localization["EmailExist"].Value);
            }
            return false;
        }
    }
}
