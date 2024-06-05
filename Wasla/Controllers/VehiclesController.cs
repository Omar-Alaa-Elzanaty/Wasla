using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wasla.Services.EntitiesServices.VehicleSerivces;

namespace Wasla.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleSrivces _vehicleSrivces;

        public VehiclesController(IVehicleSrivces vehicleSrivces)
        {
            _vehicleSrivces = vehicleSrivces;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(int id)
        {
            return Ok(await _vehicleSrivces.GetVehicleById(id));
        }

    }
}
