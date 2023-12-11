using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Helpers.Statics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Wasla.Model.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Wasla.Services.Exceptions.FilterException
{
    public class OrgPermissionAuthorizeAttribute : IAuthorizationFilter
	{
        private readonly string _permission;

		public OrgPermissionAuthorizeAttribute(string permission)
        {
                _permission = permission;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			 
			var _userManeger = (RoleManager<IdentityRole>)context.HttpContext.RequestServices.GetService(typeof(RoleManager<IdentityRole>));

			var x = _userManeger.Roles.ToList();
		}
	}
    
}
