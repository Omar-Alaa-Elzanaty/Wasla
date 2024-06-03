using Microsoft.AspNetCore.Mvc.Filters;
using Wasla.Model.Helpers.Statics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Wasla.Services.Exceptions.FilterException
{
    public class OrgPermissionAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permission;

        public OrgPermissionAuthorizeAttribute(string permission)
        {
            _permission = permission;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var _roleManager = (RoleManager<IdentityRole>)context.HttpContext.RequestServices.GetService(typeof(RoleManager<IdentityRole>));
            var userRole = context.HttpContext.User?.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault();
            var role = Task.Run(() => _roleManager.FindByNameAsync(userRole)).Result;
            if (role is not null)
            {
                var claims = Task.Run(() => _roleManager.GetClaimsAsync(role)).Result;
                var hasPermission = claims.Any(c =>
                  c.Type == PermissionsName.Org_Permission && c.Value == _permission && c.Issuer == "LOCAL AUTHORITY");
                if (!hasPermission)
                {
                    throw new ForbiddenException("There No Permission");
                }
            }

        }
    }
    
}
