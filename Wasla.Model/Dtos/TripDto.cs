using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class TripDto
    {
        /*  public OrgDriverDto Driver { get; set; }
          public float Price { get; set; }
      //    public Vehicle Vehicle { get; set; }
          public TimeSpan Duration { get; set; }
          public string From { get; set; }
          public string To { get; set; }
          public int AvailableSets { get; set; }
          public float AvailablePackageSpace { get; set; }*/
        public int Id { get; set; }
        public string OrganizationId { get; set; }
        public virtual Line Line { get; set; }=new Line();
        public float Price { get; set; }
        public TimeSpan Duration { get; set; }
        public int Points { get; set; }
        public bool IsPublic { get; set; }
        public float AdsPrice { get; set; }

     
    }
   
   
}
