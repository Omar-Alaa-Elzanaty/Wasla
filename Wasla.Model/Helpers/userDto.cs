using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
    public class userDto
    {
        public string message { get; set; }
        public bool isAuthenticated { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string phoneNumber { get; set; }

        public string Role { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
