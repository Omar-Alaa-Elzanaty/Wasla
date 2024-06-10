using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;
using Wasla.Services.Authentication.AuthServices;
using Wasla.Services.EntitiesServices.OrganizationSerivces;
using Wasla.Services.EntitiesServices.VehicleSerivces;



namespace Wasla.Api.Controllers
{
    [Route("api/organization")]
    [ApiController]
    //[Authorize(Roles ="Organization")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _orgService;
        private readonly UserManager<Account> _userManager;
        private readonly IAuthService _authservice;
        private readonly IVehicleSrivces _vehicleSrivces;
        public OrganizationController(
            IOrganizationService organizationService,
            UserManager<Account> userManager,
            IAuthService authService,
            IVehicleSrivces vehicleSrivces)
        {
            _orgService = organizationService;
            _userManager = userManager;
            _authservice = authService;
            _vehicleSrivces = vehicleSrivces;
        }
        [HttpGet("profile/{orgId}")]
        public async Task<IActionResult> OrganizationProfile([FromRoute] string orgId)
        {
            return Ok(await _orgService.GetOrgProfile(orgId));
        }
        [HttpPost("role/create")]
        //[OrgPermissionAuthorize("OrgPermissions.Role.Create.1")]
        public async Task<IActionResult> CreateOrgRole(AddOrgAdmRole addRole)
        {
            var res = await _authservice.CreateOrgRole(addRole);
            return Ok(res);
        }
        [HttpGet("roles/{userName}")]
        //[OrgPermissionAuthorize("OrgPermissions.Role.View.3")]
        public async Task<IActionResult> GetOrgRoles([FromRoute] string userName)
        {
            return Ok(await _authservice.GetOrgRoles(userName));
        }
        [HttpGet("permissions")]
        //  [OrgPermissionAuthorize("OrgPermissions.PermissionsForRole.View.3")]
        public async Task<IActionResult> GetOrgPermissions()
        {
            return Ok(await _authservice.GetAllPermissionsAsync());
        }
        [HttpGet("role/permissions/{roleName}")]
        public async Task<IActionResult> GetRoleOrgPermissions([FromRoute] string roleName)
        {
            return Ok(await _authservice.GetRolePermissions(roleName));
        }
        [HttpPost("role/permissions/create")]
        public async Task<IActionResult> CreateRolePermissions(CreateRolePermissions createRolePermissions)
        {
            return Ok(await _authservice.AddRolePermissions(createRolePermissions));
        }
        [HttpGet("{orgId}/vehicles")]
        public async Task<IActionResult> DisplayVehicles([FromRoute] string orgId)
        {
            return Ok(await _orgService.DisplayVehicles(orgId));
        }
        [HttpPost("{orgId}/vehicle/add")]
        public async Task<IActionResult> AddVehicle([FromRoute] string orgId, [FromForm] VehicleDto model)
        {
            return Ok(await _orgService.AddVehicleAsync(model, orgId));
        }
        [HttpPost("{orgId}/vehicle1/add")]
        public async Task<IActionResult> AddVehicleWithoutImage([FromRoute] string orgId, [FromBody] VehicleDto model)
        {
            return Ok(await _orgService.AddVehicleAsync(model, orgId));
        }
        [HttpPut("vehicle/update/{vehicleId}")]
        public async Task<IActionResult> UpdateVehicle(int vehicleId, [FromForm] VehicleDto model)
        {
            return Ok(await _orgService.UpdateVehicleAsync(model, vehicleId));
        }
        [HttpPut("update/profile/{orgId}")]
        public async Task<IActionResult> UpdateVehicle(string orgId, [FromForm] UpdateOrgProfileDto model)
        {
            return Ok(await _orgService.UpdateOrgProfile(orgId,model));
        }
        [HttpPut("vehicle/update1/{vehicleId}")]
        public async Task<IActionResult> UpdateVehicleWithOutImage(int vehicleId,[FromBody] VehicleDto model)
        {
            return Ok(await _orgService.UpdateVehicleAsync(model, vehicleId));
        }
        [HttpDelete("vehicle/delete/{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(int vehicleId)
        {
            return Ok(await _orgService.DeleteVehicleAsync(vehicleId));
        }
        [HttpPost("{orgId}/driver/add")]
        public async Task<IActionResult> AddDriver([FromRoute] string orgId, [FromForm] OrgDriverDto model)
        {
            return Ok(await _orgService.AddDriverAsync(model, orgId));
        }
        [HttpPost("{orgId}/driver1/add")]
        public async Task<IActionResult> AddDriverBase64([FromRoute] string orgId, [FromBody] AddOrganizationDriverDto model)
        {
            return Ok(await _orgService.AddDriverBase64Async(model, orgId));
        }
        [HttpGet("{orgId}/vehicle/vehicle-analysis")]
        public async Task<IActionResult> VehicleAnalysis(string orgId)
        {
            return Ok(await _orgService.VehicleAnalysisAsync(orgId ?? ""));
        }
        [HttpGet("getEmplyees/{orgId}")]
        public async Task<IActionResult> GetAllOrganizationEmployees(string orgId)
        {
            return Ok(await _orgService.GetEmployees(orgId));
        }
        [HttpPost("{orgId}/employee/add")]
        public async Task<IActionResult> AddEmployee([FromForm] EmployeeRegisterDto model, string orgId)
        {
            return Ok(await _orgService.AddEmployeeAsync(model, orgId));
        }
        [HttpPost("{orgId}/employee1/add")]
        public async Task<IActionResult> AddEmployeeWithoutImage([FromBody] EmployeeRegisterDto model, string orgId)
        {
            return Ok(await _orgService.AddEmployeeAsync(model, orgId));
        }
        [HttpDelete("employee/delete/{empId}")]
        public async Task<IActionResult> DeleteEmployee(string empId)
        {
            return Ok(await _orgService.DeleteEmployeeAsync(empId));
        }
        [HttpGet("{orgId}/drivers")]
        public async Task<IActionResult> GetAllDrivers(string orgId)
        {
            return Ok(await _orgService.GetAllDrivers(orgId));
        }
        #region Ads
        [HttpGet("getAds/{orgId}")]
        public async Task<IActionResult> GetAllAds(string orgId)
        {
            return Ok(await _orgService.GetAllAds(orgId));
        }
        [HttpPost("{orgId}/ads/add")]
        public async Task<IActionResult> AddAds([FromForm] AdsDto model, string orgId)
        {
            return Ok(await _orgService.AddAdsAsync(model, orgId));
        }
        [HttpPost("{orgId}/ads1/add")]
        public async Task<IActionResult> AddAdsWithoutImage([FromBody] AdsDto model, string orgId)
        {
            return Ok(await _orgService.AddAdsAsync(model, orgId));
        }
        [HttpGet("{orgId}/ads/reqeusts")]
        public async Task<IActionResult>AdsRequests(string orgId)
        {
            return Ok(await _orgService.GetAdsRequest(orgId));
        }
        [HttpPut("AddAdsToVehicles")]
        public async Task<IActionResult>AddAdsToVehicles(AddAdsToVehiclesDto model)
        {
            return Ok(await _orgService.AddAdsToVehicles(model));
        }
        [HttpPost("vehicle/{vehicleId}/ads/add/{adsId}")]
        public async Task<IActionResult> AddVehicleAds(int adsId, int vehicleId)
        {
            return Ok(await _orgService.AddAdsToVehicleAsync(adsId, vehicleId));
        }
        [HttpDelete("vehicle/{vehicleId}/ads/remove/{adsId}")]
        public async Task<IActionResult> RemoveVehicleAds(int adsId, int vehicleId)
        {
            return Ok(await _orgService.RemoveAdsFromVehicleAsync(adsId, vehicleId));
        }
        [HttpPut("ads/update/{adsId}")]
        public async Task<IActionResult> UpdateAds(int adsId, [FromForm] AdsDto model)
        {
            return Ok(await _orgService.UpdateAdsAsync(adsId, model));
        }
        [HttpPut("ads/update1/{adsId}")]
        public async Task<IActionResult> UpdateAdsWithoutImage(int adsId, [FromBody] AdsDto model)
        {
            return Ok(await _orgService.UpdateAdsAsync(adsId, model));
        }
        [HttpDelete("ads/delete/adsId")]
        public async Task<IActionResult> DeleteAds(int adsId)
        {
            return Ok(await _orgService.DeleteAdsAsync(adsId));
        }
        #endregion
        //[HttpPost("employee/add")]
        //public async Task<IActionResult> AddEmployee([FromForm] EmployeeRegisterDto model)
        //{
        //	var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //	string? orgId = null;
        //	if (userName is not null)
        //	{
        //		orgId = _userManager.FindByNameAsync(userName).Result?.Id;
        //	}

        //	return Ok(await _orgService.AddEmployeeAsync(model, orgId));
        //}
        [HttpPost("station/add/{orgId}")]
        public async Task<IActionResult> AddStation([FromRoute] string orgId, [FromBody] StationDto model)
        {

            return Ok(await _orgService.AddStationAsync(model, orgId));
        }
        [HttpPut("station/{stationId}/{orgid}")]
        public async Task<IActionResult> UpdateStation([FromRoute] int stationId, [FromRoute] string orgid, [FromBody] StationDto model) //circle
        {

            return Ok(await _orgService.UpdateStationAsync(model, orgid, stationId));
        }
        [HttpGet("stations/{orgId}")]
        public async Task<IActionResult> GetStations([FromRoute] string orgId)
        {
            return Ok(await _orgService.GetStationsAsync(orgId));
        }
        [HttpGet("station/{id}")]
        public async Task<IActionResult> GetStation([FromRoute] int id)
        {
            return Ok(await _orgService.GetStationAsync(id));
        }
        [HttpDelete("station/{id}")]
        public async Task<IActionResult> DeleteStation([FromRoute] int id)
        {
            return Ok(await _orgService.DeleteStationAsync(id));
        }
        [HttpPost("line/add")]
        public async Task<IActionResult> AddLine([FromBody] LineRequestDto model)
        {

            return Ok(await _orgService.AddLineAsync(model));
        }
        [HttpPut("line/{lineId}")]
        public async Task<IActionResult> UpdateLine([FromRoute] int lineId, [FromBody] LineRequestDto model) //circle
        {

            return Ok(await _orgService.UpdateLineAsync(model, lineId));
        }
        [HttpGet("lines/{orgId}")]
        public async Task<IActionResult> GetLines([FromRoute] string orgId)
        {
            return Ok(await _orgService.GetLinessAsync(orgId));
        }

        [HttpGet("line/{id}")]
        public async Task<IActionResult> GetLine([FromRoute] int id)
        {
            return Ok(await _orgService.GetLineAsync(id));
        }
        [HttpDelete("Line/{id}")]
        public async Task<IActionResult> DeleteLine([FromRoute] int id)
        {
            return Ok(await _orgService.DeleteLineAsync(id));
        }
        [HttpPost("trip/add/{orgId}")]
        public async Task<IActionResult> AddTrip([FromRoute] string orgId, [FromBody] AddTripDto model)
        {
            return Ok(await _orgService.AddTripAsync(model, orgId));
        }


        [HttpPut("trip/{id}")]
        public async Task<IActionResult> UpdateTrip([FromRoute] int id, [FromBody] UpdateTripDto model) //circle
        {
            return Ok(await _orgService.UpdateTripAsync(model, id));
        }
        [HttpGet("trips/{orgId}")]
        public async Task<IActionResult> GetTrips([FromRoute] string orgId)
        {
            return Ok(await _orgService.GetTripsAsync(orgId));
        }
        [HttpGet("trip/{id}")]
        public async Task<IActionResult> GetTrip([FromRoute] int id)
        {
            return Ok(await _orgService.GetTripAsync(id));
        }
        [HttpPost("tripTime/add")]
        public async Task<IActionResult> AddTripTime([FromBody] AddTripTimeDto model)
        {
            return Ok(await _orgService.AddTripTimeAsync(model));
        }
        [HttpPut("tripTime/{id}")]
        public async Task<IActionResult> UpdateTripTime([FromRoute] int id, [FromBody] UpdateTripTimeDto model) //circle
        {
            return Ok(await _orgService.UpdateTripTimeAsync(model, id));
        }
        [HttpGet("tripsTime/driver/7days")]
        public async Task<IActionResult> GetTripsForDriverForNext7DaysAsync(string orgId, string driverId, DateTime date)
        {
            return Ok(await _orgService.GetTripsForDriverForNext7DaysAsync(new TripForDriverRequestDto { OrgId = orgId, DriverId = driverId, CurrentDate = date }));
        }

        [HttpGet("nextTrip/driver")]
        public async Task<IActionResult> GetNextTripForDriverAsync(string orgId, string driverId, DateTime date)
        {
            return Ok(await _orgService.GetNextTripForDriver(orgId, driverId, date));
        }

        [HttpGet("history/trips/driver")]
        public async Task<IActionResult> GetTripsHistoryForDriverAsync(string orgId, string driverId, DateTime date)
        {
            return Ok(await _orgService.GetTripsHistoryForDriverAsync(orgId, driverId, date));
        }

        [HttpGet("tripsTime/{orgId}")]
        public async Task<IActionResult> GetTripsTime([FromRoute] string orgId)  //c
        {
            return Ok(await _orgService.GetTripsTimeAsync(orgId));
        }
        [HttpGet("tripTime/{id}")]
        public async Task<IActionResult> GetTripTime([FromRoute] int id) //c
        {
            return Ok(await _orgService.GetTripTimeAsync(id));
        }
        [HttpDelete("tripTime/{id}")]
        public async Task<IActionResult> DeleteTripTime([FromRoute] int id)
        {
            return Ok(await _orgService.DeleteTripTimeAsync(id));
        }

        [HttpGet("trip/driver/{orgId}/{driverId}")]
        //[OrgPermissionAuthorize("OrgPermissions.TestPermissions.Create.1")]
        public async Task<IActionResult> GetTripForDriver([FromRoute] string orgId, [FromRoute] string driverId)
        {
            return Ok(await _orgService.GetTripsForDriverAsync(orgId, driverId));
        }
        //I Make command in it

        [HttpGet("trips/user/{tripId}/{date}")]
        public async Task<IActionResult> GetTripstimeByDate([FromRoute] int tripId, [FromRoute] string date)
        {
            return Ok(await _orgService.GetTripsTimeByTripIdAndDate(tripId, date));
        }

        [HttpDelete("trip/{id}")]
        public async Task<IActionResult> DeleteTrip([FromRoute] int id)
        {
            return Ok(await _orgService.DeleteTripAsync(id));
        }
        [HttpGet("getOrgsByName")]
        public async Task<IActionResult> GetOrganizationaWithName(string name)
        {
            return Ok(await _orgService.GetOriganizationsWithName(name));
        }

        [HttpGet("Packages/{tripId}")]
        public async Task<IActionResult> GetPackagesAsync([FromRoute] int tripId)  //c
        {
            return Ok(await _orgService.GetPackagesTripAsync(tripId));
        }
        [HttpGet("Package/{id}")]
        public async Task<IActionResult> GetPackageAsync([FromRoute] int id) //c
        {
            return Ok(await _orgService.GetPackageAsync(id));
        }
        [HttpGet("Packages/request/{orgId}")]
        public async Task<IActionResult> GetPublicPackagesRequestAsync([FromRoute] string orgId)
        {
            return Ok(await _orgService.GetPackagesRequestAsync(orgId));
        }
        [HttpPut("Package/{packageId}/{status}")]
        public async Task<IActionResult> ReviewPackagesRequest(int packageId, PackageStatus status)
        {
            return Ok(await _orgService.ReviewPackagesRequest(packageId, status));
        }

        [HttpPost("add/seatNumber")]
        public async Task<IActionResult> IncreaseTripSeats(AddTripSeatDto tripSeat)
        {
            return Ok(await _orgService.IncreaseTripSeats(tripSeat));

        }
        [HttpGet("employee/{id}")]
        public async Task<IActionResult>GetEmployeeById(string id)
        {
            return Ok(await _orgService.GetEmployeeById(id));
        }

        [HttpGet("vehicle/{id}")]
        public async Task<IActionResult> GetVehicleBydId(int id)
        {
            return Ok(await _vehicleSrivces.GetVehicleById(id));
        }

        [HttpGet("driver/{id}")]
        public async Task<IActionResult>GetDriverById(string id)
        {
            return Ok(await _orgService.GetDriverById(id));
        }

        [HttpPut("UpdateDriverInfo")]
        public async Task<IActionResult>UpdateDriverInfo([FromForm]UpdateOrgDriverInfoDto model)
        {
            return Ok(await _orgService.UpdateDriverProfile(model));
        }
        [HttpPut("UpdateDriverInfo1")]
        public async Task<IActionResult>UpdateDriverInfoWithoutImage([FromBody]UpdateOrgDriverInfoDto model)
        {
            return Ok(await _orgService.UpdateDriverProfile(model));
        }
        [HttpDelete("Driver/{id}")]
        public async Task<IActionResult>DeleteDriverById(string id)
        {
            return Ok(await _orgService.DeleteDriverById(id));
        }
    }
}
