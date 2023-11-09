using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.AuthService;
using Wasla.Services.Exceptions.FilterException;

namespace Wasla.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authservice;
        public PassengerController(IMapper mapper,IAuthService authService)
        {
            _mapper = mapper;
            _authservice = authService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPassenger([FromBody]PassengerRegisterDto adv)
        {
               var result = await _authservice.RegisterAsync(adv);
              return Ok(result);
        }
    }
}
