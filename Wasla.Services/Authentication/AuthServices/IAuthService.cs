
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
namespace Wasla.Services.Authentication.AuthServices
{
    public interface IAuthService
    {
        Task<BaseResponse> PassengerRegisterAsync(PassengerRegisterDto Input);
        Task<BaseResponse> OrgnaizationRegisterAsync(OrgRegisterRequestDto request);
        Task<BaseResponse> DriverRegisterAsync(DriverRegisterDto model);

        Task<BaseResponse> RefreshTokenAsync(string token);
        Task<BaseResponse> LogoutAsync(string token);
        Task<BaseResponse> LoginAsync(LoginDto riderLoginDto);
        Task <BaseResponse> ResetPasswordByphoneAsync(ResetPasswordByPhoneDto resetPassword);
        Task<BaseResponse> ResetPasswordByEmailAsync(ResetPasswordByEmailDto resetPassword);
        Task<BaseResponse> CreateOrgRole(AddOrgAdmRole addRole);
        Task<BaseResponse> GetOrgRoles(string userName);
        Task<BaseResponse> GetRolePermissions(string roleName);
        Task<BaseResponse> GetAllPermissionsAsync();
        Task<BaseResponse> AddRolePermissions(CreateRolePermissions rolePermissions);
        //Task<BaseResponse> GetAllDriver();
    }
}
