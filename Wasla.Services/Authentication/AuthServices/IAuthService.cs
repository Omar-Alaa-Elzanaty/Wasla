
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

		Task<BaseResponse> SendOtpMessageAsync(string userPhone);
        Task<BaseResponse> SendOtpEmailAsync(string userEmail);

        Task<BaseResponse> CheckUserNameSimilarity(string input);
        Task<BaseResponse> CheckPhoneNumberAsync(string phoneNumber);
        Task<BaseResponse> CheckEmailAsync(string email);
		Task<BaseResponse> RefreshTokenAsync(RefTokenDto token);
        Task<BaseResponse> LogoutAsync(RefTokenDto token);
        Task<BaseResponse> LoginAsync(LoginDto riderLoginDto);
        Task<BaseResponse> CompareOtpAsync( string reciveOtp);
        Task<BaseResponse> ConfirmPhoneAsync(ConfirmNumberDto confirmNumberDto);
        Task<BaseResponse> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
        Task <BaseResponse> ResetPasswordByphoneAsync(ResetPasswordDto resetPassword);
        Task<BaseResponse> ResetPasswordByEmailAsync(ResetPasswordDto resetPassword);
        Task<BaseResponse> ChangePasswordAsync(ChangePasswordDto changePassword);
        Task<BaseResponse> CreateOrgRole(AddOrgAdmRole addRole);
        Task<BaseResponse> GetOrgRoles(string userName);
        Task<BaseResponse> GetRolePermissions(string roleName);
        Task<BaseResponse> GetAllPermissionsAsync();
        Task<BaseResponse> AddRolePermissions(CreateRolePermissions rolePermissions);
        Task<string> gnOtp();

    }
}
