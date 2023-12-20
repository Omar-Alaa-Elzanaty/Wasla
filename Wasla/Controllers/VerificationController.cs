using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.Authentication.VerifyService;

namespace Wasla.Api.Controllers
{
    [Route("api/verification")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly IVerifyService _verifyService;
        private readonly BaseResponse _response;

        public VerificationController(IVerifyService verifyService)
        {
            _verifyService = verifyService;
            _response = new();
        }

        [HttpGet("send/message")]
        public async Task<IActionResult> SendMessage([FromQuery] string phone)
        {
            var messag = await _verifyService.SendOtpMessageAsync(phone);
            _response.Data = messag;
            return Ok(_response);
        }

        [HttpGet("send/email/{email}")]//("sendEmail")]
        public async Task<IActionResult> SendEmail([FromRoute] string email)
        {
            var messag = await _verifyService.SendOtpEmailAsync(email);
            return Ok(messag);
        }

        [HttpGet("check/userName/{userName}")]
        public async Task<IActionResult> CheckUserName([FromRoute]string userName)
        {
            return Ok(await _verifyService.CheckUserNameSimilarity(userName));
        }

        [HttpGet]//("compareOtp")]
        [Route("compareOtp/{recOtp}")]
        public async Task<IActionResult> CompareOtp([FromRoute] string recOtp)
        {
            // var otp = recOtp.UserOtp;
            var res = await _verifyService.CompareOtpAsync(recOtp);
            return Ok(res);
        }
        [HttpPut("confirm/phone")]
        public async Task<IActionResult> ConfirmPhone([FromBody] ConfirmNumberDto confirmNumber)
        {
            var resualt = await _verifyService.ConfirmPhoneAsync(confirmNumber);
            return Ok(resualt);
        }
        [HttpPut("confirm/email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmail)
        {
            var resualt = await _verifyService.ConfirmEmailAsync(confirmEmail);
            return Ok(resualt);
        }

        [HttpPost("change/password/{refreshToken}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string refreshToken, [FromBody] ChangePasswordDto changePassword)
        {
            var resualt = await _verifyService.ChangePasswordAsync(refreshToken, changePassword);
            return Ok(resualt);
        }
        [HttpGet("check/phone/{phoneNumber}")]
        public async Task<IActionResult> CheckPhoneNumber(string phoneNumber)
        {
            return Ok(await _verifyService.CheckPhoneNumberAsync(phoneNumber));
        }
        [HttpGet("check/email/{email}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return Ok(await _verifyService.CheckEmailAsync(email));
        }

        [HttpPut("edit/email/{refreshToken}/{email}")]
        public async Task<IActionResult> EditEmail([FromRoute] string refreshToken, [FromRoute] string email)
        {
            return Ok(await _verifyService.EditEmailAsync(refreshToken, email));
        }
        [HttpPut("edit/phone/{refreshToken}/{phone}")]
        public async Task<IActionResult> EditPhone([FromRoute] string refreshToken, [FromRoute] string phone)
        {
            return Ok(await _verifyService.EditPhoneAsync(refreshToken, phone));
        }
    }
}
