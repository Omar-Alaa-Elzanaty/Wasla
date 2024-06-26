﻿using AutoMapper;
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
            CreateMap<Organization,OrganizationDto>().ReverseMap();
            CreateMap<Driver,DriverResponseDto>().ReverseMap();
            CreateMap<VehicleDto, Vehicle>().ReverseMap();
            CreateMap<EmployeeRegisterDto, Employee>();
            //statio

            CreateMap<Station,StationDto>().ReverseMap();
            CreateMap<PublicStation, StationDto>().ReverseMap();

            //************
            //Trip

            CreateMap<Trip,AddTripDto>().ReverseMap();
            CreateMap<Trip,UpdateTripDto>().ReverseMap();
            CreateMap<Trip,TripDto>().ReverseMap();
            CreateMap<Trip,TripForDriverDto>().ReverseMap();

            //
            CreateMap<AdsDto, Advertisment>().ReverseMap();
            CreateMap<Organization, ResponseOrgSearch>().ReverseMap();

            CreateMap<OrgDriverDto,Driver>().ReverseMap();
            CreateMap<PassangerAddAdsDto, Advertisment>().ReverseMap();

            //Line
            CreateMap<Line, LineRequestDto>().ReverseMap();/*.ForMember(dest => dest.StartId, opt => opt.MapFrom(src => src.Start.StationId))
          .ForMember(dest => dest.EndId, opt => opt.MapFrom(src => src.End.StationId));*/
           
            CreateMap<Line,LineDto>().ReverseMap();
            CreateMap<Line, LinePackagesdto>().ForMember(dest=>dest.StartStation,opt=>opt.MapFrom(src=>src.Start.Name))
                .ForMember(dest => dest.EndStation, opt => opt.MapFrom(src => src.End.Name));
            CreateMap<Line, LinePackagesReverseDto>().ForMember(dest => dest.StartStation, opt => opt.MapFrom(src => src.End.Name))
                .ForMember(dest => dest.EndStation, opt => opt.MapFrom(src => src.Start.Name));

          

            //***********************************

            //trip

            CreateMap<TripTimeTable, TripTimeDto>().ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Trip.Points))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Trip.Price))
            .ForMember(dest => dest.Line, opt => opt.MapFrom(src => src.Trip.Line))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Trip.Duration)); 

            //************************

            //Trip Time


            CreateMap<TripTimeTable,AddTripTimeDto>().ReverseMap();
            CreateMap<TripTimeTable, UpdateTripDto>().ReverseMap();
            CreateMap<TripTimeTable, TripForDriverDto>().ReverseMap();
            CreateMap<TripTimeTable, TripForUserDto>().ForMember(dest=>dest.orgName,opt=>opt.MapFrom(src=>src.Trip.Organization.Name));

            //*****************************

            //Package

            CreateMap<Package, PackagesRequestDto>().ReverseMap();
            CreateMap<Package, PackagesDto>().ForMember(dest => dest.IsStart, opt => opt.MapFrom(src => src.Trip.IsStart))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Trip.StartTime))
            .ForMember(dest => dest.ArriveTime, opt => opt.MapFrom(src => src.Trip.ArriveTime))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Trip.Trip.Duration))
                        .ForMember(dest => dest.Line, opt => opt.MapFrom(src => src.Trip.Trip.Line));
            CreateMap<Package, PublicPackagesDto>().ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Driver.StartStation.Name))
           .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Driver.EndStation.Name))
           .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver.FirstName));
            CreateMap<Package, OrgPackagesDto>().ForMember(dest => dest.IsStart, opt => opt.MapFrom(src => src.Trip.IsStart))
           .ForMember(dest => dest.TripStartTime, opt => opt.MapFrom(src => src.Trip.StartTime))
           .ForMember(dest => dest.TripArriveTime, opt => opt.MapFrom(src => src.Trip.ArriveTime))
           .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Trip.Trip.Duration))
                       .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Trip.Trip.Line.Start.Name))
                       .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Trip.Trip.Line.End.Name)).
                       ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Trip.Driver.FirstName)).
                       ForMember(dest => dest.VehicleBrand, opt => opt.MapFrom(src => src.Trip.Vehicle.Brand)).
                       ForMember(dest => dest.VehicleCategory, opt => opt.MapFrom(src => src.Trip.Vehicle.Category));
            CreateMap<Package, DriverPackagesDto>().ReverseMap();


            //****************************************
            //Public Driver
            CreateMap<PublicDriver,DriverDto>().ForMember(dest => dest.StartStation, opt => opt.MapFrom(src => src.StartStation.Name)).
                ForMember(dest => dest.EndStation, opt => opt.MapFrom(src => src.EndStation.Name));
            CreateMap<DriverRegisterDto, PublicDriver>().ReverseMap();


        }
    }
}
