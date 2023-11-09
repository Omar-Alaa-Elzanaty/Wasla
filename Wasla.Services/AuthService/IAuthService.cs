using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Dtos;
using Twilio.Rest.Api.V2010.Account;
using Wasla.Model.Helpers;

namespace Wasla.Services.AuthService
{
    public interface IAuthService
    {
       // Task<userDto> LoginAsync(LoginDto Input);
        Task<BaseResponse> RegisterAsync(PassengerRegisterDto Input);
        Task<BaseResponse> SendMessage(PhoneDto phoneNumber);
        Task<BaseResponse> RefreshTokenAsync(RefTokenDto token);
        Task<BaseResponse> LogoutAsync(RefTokenDto token);
        Task<BaseResponse> LoginAsync(RiderLoginDto riderLoginDto);
        Task<bool> CompareOtp( string reciveOtp);
        Task<BaseResponse> ConfirmPhone(ConfirmNumberDto confirmNumberDto);
        Task <BaseResponse> ResetPassword(ResetPasswordDto resetPassword);
    }
}
