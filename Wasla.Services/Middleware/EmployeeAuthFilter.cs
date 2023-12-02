using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Models;

namespace Wasla.Services.Middleware
{
	public class EmployeeAuthFilter : Attribute,IActionFilter
	{
		private readonly WaslaDb _context;
   
        public EmployeeAuthFilter(WaslaDb context)
		{
			_context=context;
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var user = _context.UserClaims.Where(c=>c.ClaimType==" ").ToList();
			var x = context.HttpContext.Request.Headers["FunctionCode"];
            Console.WriteLine("Start query");
			var z = _context.OrganizationsRegisters.Any();
            Console.WriteLine(z);

            Console.WriteLine("End query");
            Console.WriteLine(x);
        }

		public void OnActionExecuted(ActionExecutedContext context)
		{
			
		}
	}
}
