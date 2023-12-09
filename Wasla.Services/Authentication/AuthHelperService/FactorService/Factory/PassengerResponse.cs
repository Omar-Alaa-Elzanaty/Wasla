using Microsoft.EntityFrameworkCore;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory;

namespace Wasla.Services.Authentication.AuthHelperService.FactorService.Factory
{
    public class PassengerResponse : IAuthResponse
    {
        private readonly WaslaDb _db;
        public PassengerResponse(WaslaDb db)
        {
            _db = db;
        }
        public async Task<DataAuthResponse> AuthRespnseFactory(AuthResponseFactoryHelp responseHelp)
        {
            var passenger =await _db.Customers.FirstOrDefaultAsync(u => u.Id == responseHelp.userId);
            var passengerResponse = new PassengerResponseDto();
            passengerResponse.ConnectionData.Email = passenger.Email;
            passengerResponse.UserName = passenger.UserName;
            passengerResponse.ConnectionData.phone = passenger.PhoneNumber;
            passengerResponse.TokensData.Token = responseHelp.TokensData.Token;
            passengerResponse.IsAuthenticated = true;
            passengerResponse.TokensData.TokenExpiryDate = responseHelp.TokensData.TokenExpiryDate;
            passengerResponse.Role = responseHelp.role;
            passengerResponse.TokensData.RefreshToken = responseHelp.TokensData.RefreshToken;
            passengerResponse.TokensData.RefTokenExpiryDate = responseHelp.TokensData.RefTokenExpiryDate;
            passengerResponse.FirstName = passenger.FirstName;
            passengerResponse.LastName = passenger.LastName;
            passengerResponse.points = passenger.points;
            passengerResponse.PhotoUrl = passenger.PhotoUrl;

            return  passengerResponse;
            // _response.Message = _localization["LoginSuccess"].Value;
        }


    }
}
