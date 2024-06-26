﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class PackagesRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SenderId { get; set; }
        public  IFormFile ImageFile { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
        public string? ReciverUserName { get; set; }
        public string? ReciverName { get; set; }
        public string? ReciverPhoneNumber { get; set; }
        public int? TripId { get; set; }
        public string? DriverId { get; set; }
        public bool isPublic { get; set; } = false;


    }
}
