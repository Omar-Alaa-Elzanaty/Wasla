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
            var employeeResponse = new OrgEmployeeResponseDto();
            employeeResponse.ConnectionData.Email = employee.Email;
            employeeResponse.UserName = employee.UserName;
            employeeResponse.ConnectionData.phone = employee.PhoneNumber;
            employeeResponse.ConnectionData.EmailConfirmed = employee.EmailConfirmed;
            employeeResponse.ConnectionData.PhoneConfirmed = employee.PhoneNumberConfirmed;
            employeeResponse.TokensData.Token = responseHelp.TokensData.Token;
            employeeResponse.IsAuthenticated = true;
            employeeResponse.TokensData.TokenExpiryDate = responseHelp.TokensData.TokenExpiryDate;
            employeeResponse.Role = responseHelp.role;
            employeeResponse.TokensData.RefreshToken = responseHelp.TokensData.RefreshToken;
            employeeResponse.TokensData.RefTokenExpiryDate = responseHelp.TokensData.RefTokenExpiryDate;
            var role = await _roleManager.FindByNameAsync(responseHelp.role);
            employeeResponse.OrgPermissions = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            return employeeResponse;
        }
    }
}
