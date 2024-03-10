using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
    public class AuthData
    {
        public string Token { get; set; }
        public DateTime TokenExpiryDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefTokenExpiryDate { get; set; }
    }
}
