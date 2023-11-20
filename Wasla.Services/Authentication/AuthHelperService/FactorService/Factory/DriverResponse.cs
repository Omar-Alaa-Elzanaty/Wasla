using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory;

namespace Wasla.Services.Authentication.AuthHelperService.FactorService.Factory
{
    public class DriverResponse:IAuthResponse
    {
        private readonly WaslaDb _db;
        public DriverResponse(WaslaDb db) 
        {
            _db = db;
        }
        public async Task<DataAuthResponse> AuthRespnseFactory(AuthResponseFactoryHelp responseHelp)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(u => u.Id == responseHelp.userId);
            var driverResponse = new DriverResponseDto();
            driverResponse.ConnectionData.Email = driver.Email;
            driverResponse.UserName = driver.UserName;
            driverResponse.ConnectionData.phone = driver.PhoneNumber;
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
            driverResponse.Trips = driver.Trips;
            driverResponse.Rates = driver.Rates;    
            return driverResponse;
            // _response.Message = _localization["LoginSuccess"].Value;
        }


    }
}
