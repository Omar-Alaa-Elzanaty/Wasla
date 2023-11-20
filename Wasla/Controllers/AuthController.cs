using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.AdminServices;
using Wasla.Services.Authentication.AuthServices;

namespace Wasla.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
  
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authservice;
        private readonly BaseResponse _response;
        private readonly IAdminService _adminservice;

        public AuthController(IMapper mapper, IAuthService authService,IAdminService adminservice)
        {
            _mapper = mapper;
            _authservice = authService;
            _response = new();
            _adminservice = adminservice;

        }
        [HttpPost]
        public async Task<IActionResult> PassengerRegister([FromBody] PassengerRegisterDto adv)
        {
            var result = await _authservice.RegisterPassengerAsync(adv);
            return Ok(result);
        }
        //it not accept it as FromRoute make it FromBody and use class SendOtpDto or keep it 
        //https://localhost:44366/api/Auth/SendMessage?phone=%2B201118499698
        [HttpGet]
        public async Task<IActionResult> SendMessage([FromQuery]string phone)
        {
            var messag = await _authservice.SendOtpMessageAsync(phone);
            _response.Data = messag;
            return Ok(_response);
        }
     /*   [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendOtpDto input)
        {
            var messag = await _authservice.SendOtpMessageAsync(input.SendData);
            _response.Data = messag;
            return Ok(_response);
        }*/
        [HttpGet("sendEmail")]
       [Route("{email}")]
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

        [HttpGet("compareOtp")]
       [Route("{recOtp}")]
        public async Task<IActionResult> CompareOtp([FromRoute]string recOtp)
        {
           // var otp = recOtp.UserOtp;
            var res = await _authservice.CompareOtpAsync(recOtp);
            return Ok(res);
        }
        [HttpPut("confirmPhone")]
        public async Task<IActionResult> ConfirmPhone([FromBody] ConfirmNumberDto confirmNumber)
        {
            var resualt = await _authservice.ConfirmPhoneAsync(confirmNumber);
            return Ok(resualt);
        }
        [HttpPut("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmail)
        {
            var resualt = await _authservice.ConfirmEmailAsync(confirmEmail);
            return Ok(resualt);
        }
        [HttpPost("RefreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody]RefTokenDto refToken)
        {
           
            var resualt = await _authservice.RefreshTokenAsync(refToken);
            return Ok(resualt);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto riderLoginDto)
        {
            var resualt = await _authservice.LoginAsync(riderLoginDto);
            return Ok(resualt);
        }
        [HttpPost("resetPasswordByPhone")]
        public async Task<IActionResult> ResetPasswordByPhone([FromBody] ResetPasswordDto resetPassword)
        {
            var resualt = await _authservice.ResetPasswordByphoneAsync(resetPassword);
           
            return Ok(resualt);
        }
        [HttpPost("resetPasswordByEmail")]
        public async Task<IActionResult> ResetPasswordByEmail([FromBody] ResetPasswordDto resetPassword)
        {
            var resualt = await _authservice.ResetPasswordByEmailAsync(resetPassword);

            return Ok(resualt);
        }
        [HttpPost("logout")]
       [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Logout(RefTokenDto token)
        {
            var resault = await _authservice.LogoutAsync(token);
            return Ok(resault);
        }
        [HttpPost("organizationRegister")]
        public async Task<IActionResult> OrganizationRegister([FromForm] OrgRegisterRequestDto request)
        {
            return Ok(await _authservice.OrgnaizationRegisterAsync(request));
        }
        [HttpGet("confrimOrganizationAccount")]
        public async Task<IActionResult> ConfrimOrganizationAccount([FromRoute] int id)
        {
            return Ok(await _adminservice.ConfirmOrgnaizationRequestAsync(id));
        }
		[HttpGet("checkPhoneNumber")]
		public async Task<IActionResult> CheckPhoneNumber(string phoneNumber)
		{
			return Ok(await _authservice.CheckPhoneNumberAsync(phoneNumber));
		}
		[HttpGet("checkEmail")]
		public async Task<IActionResult> CheckEmail(string email)
		{
            return Ok(await _authservice.CheckEmailAsync(email));
		}
	}
}
