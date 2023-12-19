
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
        Task<BaseResponse> CompareOtpAsync(string reciveOtp);
        Task<BaseResponse> ConfirmPhoneAsync(ConfirmNumberDto confirmNumberDto);
        Task<BaseResponse> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
        Task<BaseResponse> ChangePasswordAsync(string token, ChangePasswordDto changePassword);
        Task<BaseResponse> EditPhoneAsync(string RefreshTpken, string newPhone);
        Task<BaseResponse> EditEmailAsync(string RefreshTpken, string newEmail);

    }
}
