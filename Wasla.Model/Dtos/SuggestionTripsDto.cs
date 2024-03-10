using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class SuggestionTripsDto
    {
        public int Id { get;set; }
        public string ComapnyName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double CompanyRating { get; set; }
        public double price { get; set; }
    }
}
