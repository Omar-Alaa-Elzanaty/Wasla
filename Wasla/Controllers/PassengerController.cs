using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
