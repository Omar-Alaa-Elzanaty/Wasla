﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Dtos;
using Wasla.Model.Models;

namespace Wasla.DataAccess.AutoMapping
{
    public class AuthAutoMapper : Profile
    {
        public AuthAutoMapper()
        {
            CreateMap<Customer, PassengerRegisterDto>().ReverseMap();
            CreateMap<OrgRegisterRequestDto, OrganizationRegisterRequest>().ReverseMap();
            CreateMap<OrganizationRegisterRequest, Organization>().ReverseMap();
            CreateMap<DriverRegisterDto, Driver>().ReverseMap();
            CreateMap<Driver,DriverResponseDto>().ReverseMap();
            CreateMap<VehicleDto, Vehicle>().ReverseMap();
            CreateMap<EmployeeRegisterDto, Employee>();
            CreateMap<Station,StationDto>().ReverseMap();
            CreateMap<Trip,AddTripDto>().ReverseMap();
            CreateMap<Trip,UpdateTripDto>().ReverseMap();
            CreateMap<Trip,TripDto>().ReverseMap();
            CreateMap<Trip,TripForDriverDto>().ReverseMap();
            CreateMap<AdsDto, Advertisment>().ReverseMap();
            CreateMap<Organization,ResponseOrgSearch>().ReverseMap();
        }
    }
}
