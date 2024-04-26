using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class DriverResponseDto:BaseData
    {
        public IFormFile ProfileImageFile { get; set; }
        public IFormFile LicenseImageFile { get; set; }
        public string LicenseNum { get; set; }
        public List<string> DriverPermissions { get; set; }
        public virtual DriverOrganization? Orgainzation { get; set; }
        public virtual ICollection<DriverTrip> Trips { get; set; }
        public virtual ICollection<DriverRates> Rates { get; set; }
    }
    public class DriverRates
    {
        public string DriverId { get; set; }
        public string CustomerId { get; set; }
        public Rate Rate { get; set; }
    }
    public class DriverOrganization
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string LogoUrl { get; set; }
        public float MaxWeight { get; set; }
        public float MinWeight { get; set; }
        public string? WebsiteLink { get; set; }
    }
    public class DriverTrip
    {
        public int Id { get; set; }
        public string OrganizationId { get; set; }
        public int LineId { get; set; }
        public float Price { get; set; }
        public TimeSpan Duration { get; set; }
        public int Points { get; set; }
        public bool IsPublic { get; set; }
        public float AdsPrice { get; set; }
    }
}
