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
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }

                if (!_roleManager.RoleExistsAsync(Roles.Role_Admin).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(Roles.Role_Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(Roles.Role_Driver)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(Roles.Role_Passenger)).GetAwaiter().GetResult();
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
