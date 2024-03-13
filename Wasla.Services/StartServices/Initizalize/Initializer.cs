using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Helpers.Statics;

namespace Wasla.Services.StartServices.Initizalize
{
    public class Initializer : IInitializer
    {
        private readonly WaslaDb _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Initializer(WaslaDb context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }
        public async Task Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrationsAsync().Result.Any())
                {
                    _context.Database.Migrate();
                }

                if (!await _roleManager.RoleExistsAsync(Roles.Role_Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Role_Admin));
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Role_Driver));
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Role_Passenger));
                }
            }
            catch
            {
                throw;
            }
            return;
        }
    }
}
