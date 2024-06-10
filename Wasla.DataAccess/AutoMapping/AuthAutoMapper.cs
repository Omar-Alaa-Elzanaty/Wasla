using AutoMapper;
using Wasla.Model.Dtos;
using Wasla.Model.Models;

namespace Wasla.DataAccess.AutoMapping
{
    public class AuthAutoMapper : Profile
    {
        public AuthAutoMapper()
        {
            //User

            CreateMap<Customer, PassengerRegisterDto>().ReverseMap();

            CreateMap<OrgRegisterRequestDto, OrganizationRegisterRequest>().ReverseMap();
            CreateMap<OrganizationRegisterRequest, Organization>().ReverseMap();
            CreateMap<DriverRegisterDto, Driver>().ReverseMap();
            CreateMap<Organization, OrganizationDto>().ReverseMap();
            CreateMap<Driver, DriverResponseDto>().ReverseMap();
            CreateMap<VehicleDto, Vehicle>().ReverseMap();
            CreateMap<EmployeeRegisterDto, Employee>();
            CreateMap<Customer, DisplayCustomerProfileDto>()

                .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FirstName + ' ' + src.LastName));
            CreateMap<Customer, SearchByUserNameDto>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FirstName + ' ' + src.LastName));

            //station

            CreateMap<Station, StationDto>().ReverseMap();
            CreateMap<PublicStation, StationDto>().ReverseMap();
            CreateMap<PublicStation, PublicStationRequestDto>().ReverseMap();


            //************
            //Trip
            CreateMap<PublicDriver, PubliDriverProfileDto>();
            CreateMap<Trip, AddTripDto>().ReverseMap();
            CreateMap<Trip, UpdateTripDto>().ReverseMap();
            CreateMap<Trip, TripDto>().ReverseMap();
            CreateMap<Trip, OrganizationTripResponse>();
            CreateMap<Trip, TripForDriverDto>();
            CreateMap<CreatePublicDriverCommand, PublicDriverTrip>().ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));
            //
            CreateMap<AdsDto, Advertisment>().ReverseMap();
            CreateMap<Advertisment, GetOrganizationAds>().ReverseMap();
            CreateMap<Organization, ResponseOrgSearch>().ReverseMap();

            CreateMap<OrgDriverDto, Driver>().ReverseMap();
            CreateMap<Employee, GetEmployeeOrganizationDto>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FirstName + ' ' + src.LastName));
            CreateMap<PassangerAddAdsDto, Advertisment>().ReverseMap();

            CreateMap<OrganizationRateDto, OrganizationRate>();
            CreateMap<OrganizationRegisterRequest, DisplayOrganizationRequest>();

            //Line
            CreateMap<Line, LineRequestDto>().ReverseMap();/*.ForMember(dest => dest.StartId, opt => opt.MapFrom(src => src.Start.StationId))
          .ForMember(dest => dest.EndId, opt => opt.MapFrom(src => src.End.StationId));*/

            CreateMap<Line, LineDto>().ReverseMap();
            CreateMap<Line, LinePackagesdto>().ForMember(dest => dest.StartStation, opt => opt.MapFrom(src => src.Start.Name))
                .ForMember(dest => dest.EndStation, opt => opt.MapFrom(src => src.End.Name));
            CreateMap<Line, LinePackagesReverseDto>().ForMember(dest => dest.StartStation, opt => opt.MapFrom(src => src.End.Name))
                .ForMember(dest => dest.EndStation, opt => opt.MapFrom(src => src.Start.Name));



            //***********************************

            //trip

            CreateMap<TripTimeTable, TripTimeDto>()
                .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Trip.Points))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Trip.Price))
            .ForMember(dest => dest.Line, opt => opt.MapFrom(src => src.Trip.Line))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Trip.Duration))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Vehicle.Category))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Vehicle.Brand))
            .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver.FirstName + ' ' + src.Driver.LastName));
            //************************

            //Trip Time


            CreateMap<TripTimeTable, AddTripTimeDto>().ReverseMap();
            CreateMap<TripTimeTable, UpdateTripDto>().ReverseMap();
            CreateMap<TripTimeTable, TripForDriverDto>()
                .ForMember(dest => dest.From, src => src.MapFrom(src => src.IsStart ? src.Trip.Line.Start.Name : src.Trip.Line.End.Name))
                .ForMember(dest => dest.To, src => src.MapFrom(src => src.IsStart ? src.Trip.Line.End.Name : src.Trip.Line.Start.Name))
                .ForMember(dest => dest.Price, src => src.MapFrom(src => src.Trip.Price))
                .ForMember(dest => dest.AvailablePackageSpace, src => src.MapFrom(src => src.AvailablePackageSpace))
                .ForMember(dest => dest.Duration, src => src.MapFrom(src => src.Trip.Duration))
                .ForMember(dest => dest.AvailableSets, src => src.MapFrom(src => src.Vehicle.Capcity-src.RecervedSeats.Count()));
            CreateMap<TripTimeTable, TripForUserDto>()
                .ForMember(dest => dest.orgName, opt => opt.MapFrom(src => src.Trip.Organization.Name))
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Trip.Line.Start.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Trip.Line.End.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Trip.Price))
                .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Trip.Points))
                .ForMember(dest => dest.AvailableSets, opt => opt.MapFrom(src => src.Vehicle.Capcity - src.RecervedSeats.Count));
            CreateMap<TripTimeTableStationDto, Station>();
            //*****************************

            //Package

            CreateMap<Package, PackagesRequestDto>().ReverseMap();
            CreateMap<Package, PublicPackagesDto>();
            CreateMap<Package, PackagesDto>().ForMember(dest => dest.IsStart, opt => opt.MapFrom(src => src.Trip.IsStart))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Trip.StartTime))
            .ForMember(dest => dest.ArriveTime, opt => opt.MapFrom(src => src.Trip.ArriveTime))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Trip.Trip.Duration))
                        .ForMember(dest => dest.Line, opt => opt.MapFrom(src => src.Trip.Trip.Line));
            CreateMap<Package, PublicPackagesDto>()/*.ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Driver.StartStation.Name))
           .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Driver.EndStation.Name))*/
           .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver.FirstName));
            CreateMap<Package, OrgPackagesDto>().ForMember(dest => dest.IsStart, opt => opt.MapFrom(src => src.Trip.IsStart))
           .ForMember(dest => dest.TripStartTime, opt => opt.MapFrom(src => src.Trip.StartTime))
           .ForMember(dest => dest.TripArriveTime, opt => opt.MapFrom(src => src.Trip.ArriveTime))
           .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Trip.Trip.Duration))
           .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Trip.Trip.Line.Start.Name))
           .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Trip.Trip.Line.End.Name))
           .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Trip.Driver.FirstName))
           .ForMember(dest => dest.VehicleBrand, opt => opt.MapFrom(src => src.Trip.Vehicle.Brand))
           .ForMember(dest => dest.VehicleCategory, opt => opt.MapFrom(src => src.Trip.Vehicle.Category));
            CreateMap<Package, DriverPackagesDto>().ReverseMap();
            CreateMap<PublicDriverTrip, PublicTripDto>().ForMember(dest => dest.StartStation, opt => opt.MapFrom(src => src.StartStation.Name))
                .ForMember(dest => dest.EndStation, opt => opt.MapFrom(src => src.EndStation.Name))
               .ForMember(dest => dest.PublicDriverName, opt => opt.MapFrom(src => src.PublicDriver.FirstName));

            //****************************************
            //Public Driver
            //CreateMap<PublicDriver,DriverDto>().ForMember(dest => dest.StartStation, opt => opt.MapFrom(src => src.StartStation.Name)).
            //    ForMember(dest => dest.EndStation, opt => opt.MapFrom(src => src.EndStation.Name));
            CreateMap<DriverRegisterDto, PublicDriver>().ReverseMap();
            CreateMap<PubliDriverProfileDto, PublicDriver>().ReverseMap();
            CreateMap<PublicDriverProfileVehicleDto, Vehicle>().ReverseMap();
            CreateMap<CreatePublicDriverCommand, PublicDriver>();
            CreateMap<UpdatePublicDriverProfileCommand, PublicDriver>();
            CreateMap<UpdatePublicDriverProfileVehicleCommand, Vehicle>();
            CreateMap<FollowDto, UserFollow>();
            CreateMap<FollowRequests, FollowDto>().ReverseMap();
            CreateMap<GetOrgDriverProfileDto, Driver>().ReverseMap();
            CreateMap<Organization, DriverOrganization>().ReverseMap();
            CreateMap<DriverRate, DriverRates>().ReverseMap();
            CreateMap<Trip, DriverTrip>().ReverseMap();

            CreateMap<Vehicle, TripForDriverVehicleDto>();
            CreateMap<Package, AcceptedPackagesOrgDriver>();

            CreateMap<Notification, GetAllNotificationsDto>();
            CreateMap<PublicDriverTrip, CurrentPublicDriverTripDto>();
            CreateMap<Package, PublicTripPackagesRequestDto>();
            CreateMap<PublicDriverTripRequest, PublicTripReservationDto>();
            CreateMap<TripTimeTable, CurrentOrganizationDriverTrip>();
            CreateMap<Vehicle, GetVehicleByIdDto>();
            CreateMap<Employee, GetEmployeeByIdDto>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FirstName + ' ' + src.LastName));
            CreateMap<Driver, GetDriverForOrganizationById>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FirstName + ' ' + src.LastName));

            CreateMap<PassengerPublicTripRequestDto, PublicDriverTripRequest>();
            CreateMap<Advertisment, GetAdsRequestDto>();
            CreateMap<PublicDriverTrip, PublicDriverTripHIstory>();
            CreateMap<CreatePublicDriverVehicleDto, Vehicle>();
            CreateMap<Customer, PublicTripCustomerInfoDto>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FirstName + ' ' + src.LastName));
        }
    }
}
