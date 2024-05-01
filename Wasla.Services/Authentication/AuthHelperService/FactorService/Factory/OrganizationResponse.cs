using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory;

namespace Wasla.Services.Authentication.AuthHelperService.FactorService.Factory
{
    public class OrganizationResponse:IAuthResponse
    {
        private readonly WaslaDb _db;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        public OrganizationResponse(
            WaslaDb db,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base()
        {
            _db = db;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<DataAuthResponse> AuthRespnseFactory(AuthResponseFactoryHelp responseHelp)
        {
            var organization = await _db.Organizations.FirstOrDefaultAsync(u => u.Id == responseHelp.userId);
            var organizationResponse = new OrganizationResponseDto();
            organizationResponse.UserId = responseHelp.userId;
            organizationResponse.ConnectionData.Email = organization.Email;
            organizationResponse.UserName = organization.UserName;
            organizationResponse.ConnectionData.phone = organization.PhoneNumber;
            organizationResponse.ConnectionData.EmailConfirmed = organization.EmailConfirmed;
            organizationResponse.ConnectionData.PhoneConfirmed = organization.PhoneNumberConfirmed;
            organizationResponse.TokensData.Token = responseHelp.TokensData.Token;
            organizationResponse.IsAuthenticated = true;
            organizationResponse.TokensData.TokenExpiryDate = responseHelp.TokensData.TokenExpiryDate;
            organizationResponse.Role = responseHelp.role;
            organizationResponse.TokensData.RefreshToken = responseHelp.TokensData.RefreshToken;
            organizationResponse.TokensData.RefTokenExpiryDate = responseHelp.TokensData.RefTokenExpiryDate;
            organizationResponse.TripList = _mapper.Map<List<OrganizationTripResponse>>(organization.TripList);
            organizationResponse.MaxWeight=organization.MaxWeight;
            organizationResponse.WebsiteLink = organization.WebsiteLink;
            organizationResponse.MinWeight=organization.MinWeight;
            organizationResponse.Address = organization.Address;
            organizationResponse.LogoUrl = organization.LogoUrl;
            organizationResponse.Name = organization.Name;
            var role = await _roleManager.FindByNameAsync(responseHelp.role);
            organizationResponse.OrgPermissions = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            return organizationResponse;
        }
    }
}
