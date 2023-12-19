
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
namespace Wasla.Services.Authentication.AuthServices
{
    public interface IAuthService
    {
       // Task<userDto> LoginAsync(LoginDto Input);
        Task<BaseResponse> PassengerRegisterAsync(PassengerRegisterDto Input);
        Task<BaseResponse> OrgnaizationRegisterAsync(OrgRegisterRequestDto request);
        Task<BaseResponse> DriverRegisterAsync(DriverRegisterDto model);

       // Task<BaseResponse> SendOtpMessageAsync(string userPhone);
        //Task<BaseResponse> SendOtpEmailAsync(string userEmail);

       // Task<BaseResponse> CheckUserNameSimilarity(string input);
       // Task<BaseResponse> CheckPhoneNumberAsync(string phoneNumber);
       // Task<BaseResponse> CheckEmailAsync(string email);
		Task<BaseResponse> RefreshTokenAsync(string token);
        Task<BaseResponse> LogoutAsync(string token);
        Task<BaseResponse> LoginAsync(LoginDto riderLoginDto);
       // Task<BaseResponse> CompareOtpAsync( string reciveOtp);
      //  Task<BaseResponse> ConfirmPhoneAsync(ConfirmNumberDto confirmNumberDto);
      //  Task<BaseResponse> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
        Task <BaseResponse> ResetPasswordByphoneAsync(ResetPasswordByPhoneDto resetPassword);
        Task<BaseResponse> ResetPasswordByEmailAsync(ResetPasswordByEmailDto resetPassword);
      //  Task<BaseResponse> ChangePasswordAsync(string token,ChangePasswordDto changePassword);
        Task<BaseResponse> CreateOrgRole(AddOrgAdmRole addRole);
        Task<BaseResponse> GetOrgRoles(string userName);
        Task<BaseResponse> GetRolePermissions(string roleName);
        Task<BaseResponse> GetAllPermissionsAsync();
        Task<BaseResponse> AddRolePermissions(CreateRolePermissions rolePermissions);

    }
}
