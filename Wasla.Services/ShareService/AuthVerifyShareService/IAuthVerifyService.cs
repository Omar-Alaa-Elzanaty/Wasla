using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Services.ShareService.AuthVerifyShareService
{
    public interface IAuthVerifyService
    {
        Task<Account> getUserByToken(string token);
        Task<Account> getUserByPhone(string phoneNumber);
        Task<Account> getUserByEmail(string email);
        Task<bool> CheckPhoneNumber(string PhoneNumbe);
        Task<bool> CheckEmail(string Email);
    }
}
