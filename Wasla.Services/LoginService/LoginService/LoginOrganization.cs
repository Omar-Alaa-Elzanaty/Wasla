using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;

namespace Wasla.Services.LoginService.LoginService
{
    public class LoginOrganization
    {
        private readonly WaslaDb _db;
        public LoginOrganization(WaslaDb db) : base()
        {
            _db = db;
        }
        public async Task<object> Login(LoginHelp loginHelp)
        {
            var organization = await _db.Organizations.FirstOrDefaultAsync(u => u.Id == loginHelp.userId);
            var organizationResponse = new OrganizationResponseDto();
            organizationResponse.ConnectionData.Email = organization.Email;
            organizationResponse.UserName = organization.UserName;
            organizationResponse.ConnectionData.phone = organization.PhoneNumber;
            organizationResponse.TokensData.Token = loginHelp.TokensData.Token;
            organizationResponse.IsAuthenticated = true;
            organizationResponse.TokensData.TokenExpiryDate = loginHelp.TokensData.TokenExpiryDate;
            organizationResponse.Role = loginHelp.role;
            organizationResponse.TokensData.RefreshToken = loginHelp.TokensData.RefreshToken;
            organizationResponse.TokensData.RefTokenExpiryDate = loginHelp.TokensData.RefTokenExpiryDate;
            organizationResponse.TripList = organization.TripList;
            organizationResponse.MaxWeight=organization.MaxWeight;
            organizationResponse.WebsiteLink = organization.WebsiteLink;
            organizationResponse.MinWeight=organization.MinWeight;
            organizationResponse.Address = organization.Address;
            organizationResponse.LogoUrl = organization.LogoUrl;
            organizationResponse.Name = organization.Name;
             return organizationResponse;
        }
    }
}
