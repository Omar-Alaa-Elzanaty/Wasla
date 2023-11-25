using AutoMapper;
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
        }
    }
}
