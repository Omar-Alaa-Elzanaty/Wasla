using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.AdminServices;
using Wasla.Services.Authentication.AuthServices;

namespace Wasla.Api.Controllers
{
	[Route("api/auth")]
	[ApiController]

	public class AuthController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IAuthService _authservice;
		private readonly BaseResponse _response;
		private readonly IAdminService _adminservice;
		private readonly WaslaDb _context;

		public AuthController(IMapper mapper, IAuthService authService, IAdminService adminservice, WaslaDb context)
		{
			_mapper = mapper;
			_authservice = authService;
			_response = new();
			_adminservice = adminservice;
			_context = context;
		}
		[HttpPost("passenger/register")]
        public async Task<IActionResult> PassengerRegister([FromBody] PassengerRegisterDto adv)
        {
            var result = await _authservice.PassengerRegisterAsync(adv);
            return Ok(result);
        }
        //https://localhost:44366/api/Auth/SendMessage?phone=%2B201118499698
        [HttpGet("sendMessage")]
        public async Task<IActionResult> SendMessage([FromQuery] string phone)
        {
            var messag = await _authservice.SendOtpMessageAsync(phone);
            _response.Data = messag;
            return Ok(_response);
        }
        
        [HttpGet("send-email/{email}")]//("sendEmail")]
        public async Task<IActionResult> SendEmail([FromRoute] string email)
        {
            var messag = await _authservice.SendOtpEmailAsync(email);
            return Ok(messag);
        }

        [HttpGet("checkUserName")]
        public async Task<IActionResult> CheckUserName(string userName)
        {
            return Ok(await _authservice.CheckUserNameSimilarity(userName));
        }

        [HttpGet]//("compareOtp")]
        [Route("compare-Otp/{recOtp}")]
        public async Task<IActionResult> CompareOtp([FromRoute] string recOtp)
        {
            // var otp = recOtp.UserOtp;
            var res = await _authservice.CompareOtpAsync(recOtp);
            return Ok(res);
        }
        [HttpPut("confirm-phone")]
        public async Task<IActionResult> ConfirmPhone([FromBody] ConfirmNumberDto confirmNumber)
        {
            var resualt = await _authservice.ConfirmPhoneAsync(confirmNumber);
            return Ok(resualt);
        }
        [HttpPut("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmail)
        {
            var resualt = await _authservice.ConfirmEmailAsync(confirmEmail);
            return Ok(resualt);
        }
        [HttpPost("refreshToken/{refreshToken}")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromRoute] string refreshToken)
        {

            var resualt = await _authservice.RefreshTokenAsync(refreshToken);
            return Ok(resualt);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto riderLoginDto)
        {
            var resualt = await _authservice.LoginAsync(riderLoginDto);
            return Ok(resualt);
        }
        [HttpPost("resetPasswordByPhone")]
        public async Task<IActionResult> ResetPasswordByPhone([FromBody] ResetPasswordByPhoneDto resetPassword)
        {
            var resualt = await _authservice.ResetPasswordByphoneAsync(resetPassword);

            return Ok(resualt);
        }
        [HttpPost("reset-password/email")]
        public async Task<IActionResult> ResetPasswordByEmail([FromBody] ResetPasswordByEmailDto resetPassword)
        {
            var resualt = await _authservice.ResetPasswordByEmailAsync(resetPassword);

            return Ok(resualt);
        }
       
        [HttpPost("change-password/{refreshToken}")]
        public async Task<IActionResult> ChangePasswordBy([FromRoute] string refreshToken,[FromBody] ChangePasswordDto changePassword)
        {
            var resualt = await _authservice.ChangePasswordAsync(refreshToken,changePassword);
            return Ok(resualt);
        }
        
        [HttpPost("logout/{refreshToken}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            var resault = await _authservice.LogoutAsync(refreshToken);
            return Ok(resault);
        }
        [HttpPost("organization/register")]
        public async Task<IActionResult> OrganizationRegister([FromForm] OrgRegisterRequestDto request)
        {
            return Ok(await _authservice.OrgnaizationRegisterAsync(request));
        }
        [HttpGet("organization/account/confirm/{id}")]
        public async Task<IActionResult> ConfrimOrganizationAccount(int id)
        {
            return Ok(await _adminservice.ConfirmOrgnaizationRequestAsync(id));
        }
        [HttpGet("check-phone/{phoneNumber}")]
        public async Task<IActionResult> CheckPhoneNumber(string phoneNumber)
        {
            return Ok(await _authservice.CheckPhoneNumberAsync(phoneNumber));
        }
        [HttpGet("check-email/{email}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return Ok(await _authservice.CheckEmailAsync(email));
		}
        [HttpPost("Role/create")]
        public async Task<IActionResult> CreateRole(AddOrgAdmRole addRole)
        {
            var res = await _authservice.CreateOrgRole(addRole);
            return Ok(res);
        }
        [HttpGet("org/roles")]
        public async Task<IActionResult> GetOrgRoles(string userName)
        {
            return Ok(await _authservice.GetOrgRoles(userName));
        }
        [HttpGet("org/permissions")]
        public async Task<IActionResult> GetOrgPermissions()
        {
            return Ok(await _authservice.GetAllPermissionsAsync());
        }
        [HttpGet("org/role/permissions/{roleName}")]
        public async Task<IActionResult> GetRoleOrgPermissions([FromRoute]string roleName)
        {
            return Ok(await _authservice.GetRolePermissions(roleName));
        }
        [HttpPost("role/permissions/create")]
        public async Task<IActionResult> CreateRolePermissions(CreateRolePermissions createRolePermissions)
        {
            return Ok(await _authservice.AddRolePermissions(createRolePermissions));
        }
        [HttpGet("opt/generate")]
        public async Task<IActionResult> genOtp()
        {
            return Ok(await _authservice.gnOtp());
        }
    }
}
