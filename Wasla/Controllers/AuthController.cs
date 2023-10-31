using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.Net;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.AuthService;
using Wasla.Services.Exceptions;
using Wasla.Services.Exceptions.FilterException;

namespace Wasla.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authservice;
        private readonly BaseResponse _response;

        public AuthController(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authservice = authService;
            _response = new();

        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> sendMessage([FromBody] PhoneDto phoneNumber)
        {
            var messag = await _authservice.SendMessage(phoneNumber);
            _response.Result = messag;
            return Ok(_response);
        }
        
        [HttpGet]
        [Route("{recOtp}")]
        public async Task<ActionResult<BaseResponse>> CompareOtp([FromRoute]string recOtp)
        {
           // var otp = recOtp.UserOtp;
            var res = await _authservice.CompareOtp(recOtp);
            _response.Result =res;
            return Ok(_response);
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> ConfirmNumber([FromBody] ConfirmNumberDto confirmNumber)
        {
            var resualt = await _authservice.ConfirmPhone(confirmNumber);
            _response.Result= resualt;
            return Ok(_response);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> RefreshToken([FromBody]RefTokenDto refToken)
        {
           
            var resualt = await _authservice.RefreshTokenAsync(refToken);
            _response.Result= resualt;
            return Ok(_response);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<BaseResponse>> Login([FromBody] RiderLoginDto riderLoginDto)
        {
            var resualt = await _authservice.LoginAsync(riderLoginDto);
            _response.Result = resualt;
            return Ok(_response);
        }
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<BaseResponse>> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            var resualt = await _authservice.ResetPassword(resetPassword);
            _response.Result = resualt;
            return Ok(_response);
        }
        [HttpPost("Logout")]
       [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<BaseResponse>> Logout(RefTokenDto token)
        {
            var resault = await _authservice.LogoutAsync(token);
            _response.Result = resault;
            return Ok(_response);
        }
    }
}
