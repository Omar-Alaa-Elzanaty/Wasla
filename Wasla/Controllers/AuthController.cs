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
        [HttpPost("resetPasswordByEmail")]
        public async Task<IActionResult> ResetPasswordByEmail([FromBody] ResetPasswordByEmailDto resetPassword)
        {
            var resualt = await _authservice.ResetPasswordByEmailAsync(resetPassword);

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
      

      
    }
}
