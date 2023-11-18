using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Dtos;
using Twilio.Rest.Api.V2010.Account;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Services.AuthServices
{
    public interface IAuthService
    {
       // Task<userDto> LoginAsync(LoginDto Input);
        Task<BaseResponse> OrgnaizationRegisterAsync(OrgRegisterRequestDto request);
        Task<BaseResponse> DriverRegisterAsync(DriverRegisterDto model);

		Task<BaseResponse> SendOtpMessageAsync(string userPhone);
        Task<BaseResponse> RegisterPassengerAsync(PassengerRegisterDto Input);
        Task<BaseResponse> SendOtpEmailAsync(string userEmail);

        Task<BaseResponse> CheckUserNameSimilarity(string input);
		Task<BaseResponse> RefreshTokenAsync(RefTokenDto token);
        Task<BaseResponse> LogoutAsync(RefTokenDto token);
        Task<BaseResponse> LoginAsync(LoginDto riderLoginDto);
        Task<BaseResponse> CompareOtpAsync( string reciveOtp);
        Task<BaseResponse> ConfirmPhoneAsync(ConfirmNumberDto confirmNumberDto);
        Task<BaseResponse> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
        Task <BaseResponse> ResetPasswordByphoneAsync(ResetPasswordDto resetPassword);
        Task<BaseResponse> ResetPasswordByEmailAsync(ResetPasswordDto resetPassword);

    }
}
