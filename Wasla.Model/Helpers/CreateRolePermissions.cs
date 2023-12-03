using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
    public class CreateRolePermissions
    {
        public string RoleId { get; set; }
        public List<string> RolePermissions { get; set; }
    }
}
