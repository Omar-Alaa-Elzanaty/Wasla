using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;

namespace Wasla.Model.Dtos
{
    public class PassengerResponseDto:DataAuthResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhotoUrl { get; set; }
        public bool Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public int points { get; set; }
    }
}
