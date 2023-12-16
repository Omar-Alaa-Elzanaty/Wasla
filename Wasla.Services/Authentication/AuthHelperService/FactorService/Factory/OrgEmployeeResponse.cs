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
using Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory;

namespace Wasla.Services.Authentication.AuthHelperService.FactorService.Factory
{
    public class OrgEmployeeResponse : IAuthResponse
    {
        private readonly WaslaDb _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        public OrgEmployeeResponse(WaslaDb db, RoleManager<IdentityRole> roleManager) : base()
        {
            _db = db;
            _roleManager = roleManager;
        }
        public async Task<DataAuthResponse> AuthRespnseFactory(AuthResponseFactoryHelp responseHelp)
        {
            var employee = await _db.Organizations.FirstOrDefaultAsync(u => u.Id == responseHelp.userId);
            var organizationResponse = new OrgEmployeeResponseDto();
            organizationResponse.ConnectionData.Email = employee.Email;
            organizationResponse.UserName = employee.UserName;
            organizationResponse.ConnectionData.phone = employee.PhoneNumber;
            organizationResponse.TokensData.Token = responseHelp.TokensData.Token;
            organizationResponse.IsAuthenticated = true;
            organizationResponse.TokensData.TokenExpiryDate = responseHelp.TokensData.TokenExpiryDate;
            organizationResponse.Role = responseHelp.role;
            organizationResponse.TokensData.RefreshToken = responseHelp.TokensData.RefreshToken;
            organizationResponse.TokensData.RefTokenExpiryDate = responseHelp.TokensData.RefTokenExpiryDate;
            var role = await _roleManager.FindByNameAsync(responseHelp.role);
            organizationResponse.OrgPermissions = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            return organizationResponse;
        }
    }
}
