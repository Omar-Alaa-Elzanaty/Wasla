using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wasla.Services.PassangerServices;
namespace Wasla.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPassangerService _passangerService;
		public PassengerController(IMapper mapper, IPassangerService passangerService)
		{
			_mapper = mapper;
			_passangerService = passangerService;
		}

		[HttpGet]
		public async Task<IActionResult> SetsStatus()
		{
			return Ok(await _passangerService.SetsRecordsAsync(0));
		}
	}
}
