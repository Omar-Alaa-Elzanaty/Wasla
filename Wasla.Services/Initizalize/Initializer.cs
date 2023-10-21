using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;

namespace Wasla.Services.Initizalize
{
	public class Initializer : IInitializer
	{
		private readonly WaslaDb _context;
        public Initializer(WaslaDb context)
        {
            _context= context;
        }
        public async Task Initialize()
		{
			try
			{
				if (_context.Database.GetPendingMigrations().Any())
				{
					_context.Database.Migrate();
				}
				else return;
			}
			catch
			{
				throw;
			}
		}
	}
}
