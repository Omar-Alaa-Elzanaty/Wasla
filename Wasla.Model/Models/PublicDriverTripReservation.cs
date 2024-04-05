using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
    public class PublicDriverTripReservation
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int PublicDriverTripId { get; set; }
        public virtual PublicDriverTrip PublicDriverTrip { get; set; }
    }
}
