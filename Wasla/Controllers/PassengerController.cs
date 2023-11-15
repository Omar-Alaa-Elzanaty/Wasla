using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.AuthServices;
using Wasla.Services.Exceptions.FilterException;

namespace Wasla.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IMapper _mapper;
        public PassengerController(IMapper mapper)
        {
            _mapper = mapper;
        }
      
    }
}
