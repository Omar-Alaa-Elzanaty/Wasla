using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Models;
using Wasla.Services.Exceptions.FilterException;

namespace Wasla.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestPermissionsController : ControllerBase
    {
        public List<string> Tests = new List<string>();
        public TestPermissionsController()
        {
        }
        [HttpGet]
        //[OrgPermissionAuthorizeAttribute("OrgPermissions.TestPermissions.View.3")]//.View.3")]
        //   [ServiceFilter(typeof(OrgPermissionAuthorizeAttribute),Argumen new object[] { "YourPermission" })]

        public IActionResult GetTrips()

        {
            return Ok(Tests);
        }

        [HttpPost]
       // [OrgPermissionAuthorizeAttribute("OrgPermissions.TestPermissions.Create.1"]
        public IActionResult AddTrip(string test)
        {
            if (test == null)
            {
                return BadRequest("Invalid test data");
            }

            Tests.Add(test);
            return Ok();
        }


    }
}