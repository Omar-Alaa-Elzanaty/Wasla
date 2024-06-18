
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;

namespace Wasla.Services.Authentication.VerifyService
{
    public interface IVerifyService
    {
        Task<BaseResponse> SendOtpMessageAsync(string userPhone);
        Task<BaseResponse> SendOtpEmailAsync(string userEmail);
        Task<BaseResponse> CheckUserNameSimilarity(string input);
        Task<BaseResponse> CheckPhoneNumberAsync(string phoneNumber);
        Task<BaseResponse> CheckEmailAsync(string email);
        Task<BaseResponse> CheckPhoneNumberForEditAsync(string phoneNumber,string userId);
        Task<BaseResponse> CheckEmailForEditAsync(string email,string userId);
        Task<BaseResponse> CompareOtpAsync(string reciveOtp);
        Task<BaseResponse> ConfirmPhoneAsync(ConfirmNumberDto confirmNumberDto);
        Task<BaseResponse> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
        Task<BaseResponse> ChangePasswordAsync(ChangePasswordDto changePassword);
        Task<BaseResponse> EditPhoneAsync(EditPhoneDto Phone);
        Task<BaseResponse> EditEmailAsync(EditEmailDto Email);
        Task<string> SetOtp();

    }
}
