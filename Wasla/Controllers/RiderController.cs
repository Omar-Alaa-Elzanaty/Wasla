using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.AuthService;
using Wasla.Services.Exceptions;
using Wasla.Services.Exceptions.FilterException;

namespace Wasla.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public class RiderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authservice;
        private readonly BaseResponse _response;
        public RiderController(IMapper mapper,IAuthService authService)
        {
            _mapper = mapper;
            _authservice = authService;
            _response = new();
        }

        [HttpPost("Register")]
        public async Task<ActionResult<BaseResponse>> AddRider([FromBody]RiderRegisterDto adv)
        {
            
                if (!ModelState.IsValid)
                {
                    throw new BadRequestException("there is error");
                }
                var result = await _authservice.RegisterAsync(adv);
                if (result is not null)
                {
                    _response.Result = result;
                    return Ok(_response);
                }
               throw new BadRequestException("there is error");
        }

    }
}
