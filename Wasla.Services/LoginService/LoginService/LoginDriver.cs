using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.LoginService.ILoginService;

namespace Wasla.Services.LoginService.LoginService
{
    public class LoginDriver:ILogin
    {
        private readonly WaslaDb _db;
        public LoginDriver(WaslaDb db) : base()
        {
            _db = db;
        }
        public async Task<DataAuthResponse> Login(LoginHelp loginHelp)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(u => u.Id == loginHelp.userId);
            var driverResponse = new DriverResponseDto();
            driverResponse.ConnectionData.Email = driver.Email;
            driverResponse.UserName = driver.UserName;
            driverResponse.ConnectionData.phone = driver.PhoneNumber;
            driverResponse.TokensData.Token = loginHelp.TokensData.Token;
            driverResponse.IsAuthenticated = true;
            driverResponse.TokensData.TokenExpiryDate = loginHelp.TokensData.TokenExpiryDate;
            driverResponse.Role = loginHelp.role;
            driverResponse.TokensData.RefreshToken = loginHelp.TokensData.RefreshToken;
            driverResponse.TokensData.RefTokenExpiryDate = loginHelp.TokensData.RefTokenExpiryDate;
            driverResponse.FirstName = driver.FirstName;
            driverResponse.LastName = driver.LastName;
            driverResponse.PhotoUrl = driver.PhotoUrl;
            driverResponse.License = driver.License;
            driverResponse.Orgainzation = driver.Orgainzation;
            driverResponse.Trips = driver.Trips;
            driverResponse.Rates = driver.Rates;    
            return driverResponse;
            // _response.Message = _localization["LoginSuccess"].Value;
        }


    }
}
