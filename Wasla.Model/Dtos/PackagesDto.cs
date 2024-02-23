using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class PackagesDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SenderId { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
        public string? ReciverUserName { get; set; }
        public string? ReciverName { get; set; }
        public string? ReciverPhoneNumber { get; set; }
        //tripTimeTable
        public virtual Line Line { get; set; } = new Line();
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ArriveTime { get; set; }
        public bool IsStart { get; set; }
        //
        //public virtual TripTimeTable? Trip { get; set; }=new TripTimeTable();
    }
}
