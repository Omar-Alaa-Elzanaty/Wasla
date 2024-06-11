using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class GetPublicTripPassenger
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public bool OnRoad { get; set; }
        public string LocationDescription { get; set; }
        public  PublicTripPassengerDto Customer { get; set; }
        public int PublicDriverTripId { get; set; }
    }
    public class PublicTripPassengerDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? PhotoUrl { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
