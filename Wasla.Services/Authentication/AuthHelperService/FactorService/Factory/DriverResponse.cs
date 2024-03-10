using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory;

namespace Wasla.Services.Authentication.AuthHelperService.FactorService.Factory
{
    public class DriverResponse:IAuthResponse
    {
        private readonly WaslaDb _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DriverResponse(WaslaDb db,RoleManager<IdentityRole> roleManager) 
        {
            _db = db;
            _roleManager = roleManager;
        }
        public async Task<DataAuthResponse> AuthRespnseFactory(AuthResponseFactoryHelp responseHelp)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(u => u.Id == responseHelp.userId);
            var driverResponse = new DriverResponseDto();
            driverResponse.ConnectionData.Email = driver.Email;
            driverResponse.UserName = driver.UserName;
            driverResponse.ConnectionData.phone = driver.PhoneNumber;
            driverResponse.ConnectionData.EmailConfirmed = driver.EmailConfirmed;
            driverResponse.ConnectionData.PhoneConfirmed = driver.PhoneNumberConfirmed;
            driverResponse.TokensData.Token = responseHelp.TokensData.Token;
            driverResponse.IsAuthenticated = true;
            driverResponse.TokensData.TokenExpiryDate = responseHelp.TokensData.TokenExpiryDate;
            driverResponse.Role = responseHelp.role;
            driverResponse.TokensData.RefreshToken = responseHelp.TokensData.RefreshToken;
            driverResponse.TokensData.RefTokenExpiryDate = responseHelp.TokensData.RefTokenExpiryDate;
            driverResponse.FirstName = driver.FirstName;
            driverResponse.LastName = driver.LastName;
            driverResponse.PhotoUrl = driver.PhotoUrl;
            driverResponse.LicenseNum = driver.LicenseNum;
            driverResponse.Orgainzation = driver.Orgainzation;
            //driverResponse.Trips = driver.Trips;
            driverResponse.Rates = driver.Rates;
            if(driver.Orgainzation != null)
            {
                var role = await _roleManager.FindByNameAsync(responseHelp.role);
                driverResponse.DriverPermissions = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            }
            return driverResponse;
        }
    }
}
