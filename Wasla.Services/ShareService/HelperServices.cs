using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Services.ShareService
{
    public static class HelperServices
    {
        public static string CollectIdentityResultErrors(IdentityResult? result)
        {
            string errors = string.Empty;
            foreach (var error in result.Errors)
            {
                errors += error + ", ";
            }
            return errors;
        }
    }
}
