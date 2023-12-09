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
    public class OrgPermissionAuthorizeAttribute : ActionFilterAttribute,IActionFilter
	{
        private readonly string _permission;

		public OrgPermissionAuthorizeAttribute(string permission)
            {
                _permission = permission;
		}
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        
            var hasPermission = context.HttpContext.User.Claims.Any(c =>
                c.Type == PermissionsName.Org_Permission && c.Value == _permission && c.Issuer == "LOCAL AUTHORITY");

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var _userManeger = (UserManager<Account>)context.HttpContext.RequestServices.GetService(typeof(UserManager<Account>));
			var x = _userManeger.Users.ToList();

			base.OnActionExecuting(context);
		}
	}
    
}
