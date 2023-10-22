using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
        public ValuesController()
        {
            
        }
        [HttpGet()]
        public async Task<IActionResult> test()
        {
            return Ok(" test ");
        }
    }
}
