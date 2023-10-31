using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Twilio.Rest.Api.V2010.Account;

namespace Wasla.Services.AuthService
{
    public interface IAuthService
    {
       // Task<userDto> LoginAsync(LoginDto Input);
        Task<string> RegisterAsync(RiderRegisterDto Input);
        Task<MessageResource> SendMessage(PhoneDto phoneNumber);
        Task<userDto> RefreshTokenAsync(RefTokenDto token);
        Task<string> LogoutAsync(RefTokenDto token);
        Task<userDto> LoginAsync(RiderLoginDto riderLoginDto);
        Task<bool> CompareOtp( string reciveOtp);
        Task<userDto> ConfirmPhone(ConfirmNumberDto confirmNumberDto);
        Task <string> ResetPassword(ResetPasswordDto resetPassword);
    }
}
