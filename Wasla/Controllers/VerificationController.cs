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

        [HttpGet("sendMessage")]
        public async Task<IActionResult> SendMessage([FromQuery] string phone)
        {
            var messag = await _verifyService.SendOtpMessageAsync(phone);
            _response.Data = messag;
            return Ok(_response);
        }

        [HttpGet("sendEmail/{email}")]//("sendEmail")]
        public async Task<IActionResult> SendEmail([FromRoute] string email)
        {
            var messag = await _verifyService.SendOtpEmailAsync(email);
            return Ok(messag);
        }

        [HttpGet("checkUserName")]
        public async Task<IActionResult> CheckUserName(string userName)
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
        [HttpPut("confirmPhone")]
        public async Task<IActionResult> ConfirmPhone([FromBody] ConfirmNumberDto confirmNumber)
        {
            var resualt = await _verifyService.ConfirmPhoneAsync(confirmNumber);
            return Ok(resualt);
        }
        [HttpPut("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmail)
        {
            var resualt = await _verifyService.ConfirmEmailAsync(confirmEmail);
            return Ok(resualt);
        }

        [HttpPost("changePassword/{refreshToken}")]
        public async Task<IActionResult> ChangePasswordBy([FromRoute] string refreshToken, [FromBody] ChangePasswordDto changePassword)
        {
            var resualt = await _verifyService.ChangePasswordAsync(refreshToken, changePassword);
            return Ok(resualt);
        }
        [HttpGet("checkPhone/{phoneNumber}")]
        public async Task<IActionResult> CheckPhoneNumber(string phoneNumber)
        {
            return Ok(await _verifyService.CheckPhoneNumberAsync(phoneNumber));
        }
        [HttpGet("checkEmail/{email}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return Ok(await _verifyService.CheckEmailAsync(email));
        }

        [HttpPut("editEmail/{refreshToken}/{email}")]
        public async Task<IActionResult> EditEmail([FromRoute] string refreshToken, [FromRoute] string email)
        {
            return Ok(await _verifyService.EditEmailAsync(refreshToken, email));
        }
        [HttpPut("editPhone/{refreshToken}/{phone}")]
        public async Task<IActionResult> EditPhone([FromRoute] string refreshToken, [FromRoute] string phone)
        {
            return Ok(await _verifyService.EditPhoneAsync(refreshToken, phone));
        }
    }
}
