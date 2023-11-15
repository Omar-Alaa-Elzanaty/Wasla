using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.Exceptions;
using Wasla.Services.LoginService.ILoginService;

namespace Wasla.Services.LoginService.LoginService
{
    public class LoginPassenger : ILogin
    {
        private readonly WaslaDb _db;
        public LoginPassenger(WaslaDb db):base()
        {
            _db = db;
        }
        public async Task<object> Login(LoginHelp loginHelp)
        {
            var passenger =await _db.Customers.FirstOrDefaultAsync(u => u.Id == loginHelp.userId);
            var passengerResponse = new PassengerResponseDto();
            passengerResponse.ConnectionData.Email = passenger.Email;
            passengerResponse.UserName = passenger.UserName;
            passengerResponse.ConnectionData.phone = passenger.PhoneNumber;
            passengerResponse.TokensData.Token = loginHelp.TokensData.Token;
            passengerResponse.IsAuthenticated = true;
            passengerResponse.TokensData.TokenExpiryDate = loginHelp.TokensData.TokenExpiryDate;
            passengerResponse.Role = loginHelp.role;
            passengerResponse.TokensData.RefreshToken = loginHelp.TokensData.RefreshToken;
            passengerResponse.TokensData.RefTokenExpiryDate = loginHelp.TokensData.RefTokenExpiryDate;
            passengerResponse.FirstName = passenger.FirstName;
            passengerResponse.LastName = passenger.LastName;
            passengerResponse.points = passenger.points;
            passengerResponse.PhotoUrl = passenger.PhotoUrl;
            return  passengerResponse;
            // _response.Message = _localization["LoginSuccess"].Value;
        }


    }
}
