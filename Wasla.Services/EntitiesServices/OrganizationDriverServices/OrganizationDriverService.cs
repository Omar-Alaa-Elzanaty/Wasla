using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Helpers.Statics;
using Wasla.Model.Models;
using Wasla.Services.HlepServices.MediaSerivces;

namespace Wasla.Services.EntitiesServices.OrganizationDriverServices
{
    public class OrganizationDriverService : IOrganizationDriverService
    {
        private readonly WaslaDb _context;
        private readonly BaseResponse _response;
        private readonly IStringLocalizer<OrganizationDriverService> _localization;
        private readonly IMapper _mapper;
        private readonly IMediaSerivce _mediaSerivce;
        private readonly UserManager<Account> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public OrganizationDriverService(
            WaslaDb context,
            IStringLocalizer<OrganizationDriverService> localization,
            IMapper mapper,
            IMediaSerivce mediaSerivce,
            UserManager<Account> userManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContext)
        {
            _context = context;
            _response = new();
            _localization = localization;
            _mapper = mapper;
            _mediaSerivce = mediaSerivce;
            _userManager = userManager;
            _httpContext = httpContext;
        }

        public async Task<BaseResponse> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.BadRequest, _localization["Unauthorized"].Value);
            }

            var entity = await _context.Drivers.FindAsync(user.Id);

            if (entity is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.BadRequest, _localization["Unauthorized"].Value);
            }

            var driverProfile = _mapper.Map<GetOrgDriverProfileDto>(entity);

            _response.Data = driverProfile;

            return _response;
        }

        public async Task<BaseResponse> DecreaseSeatByOneAsync(DecreaseOrgTripByOneCommnad command)
        {
            var trip = await _context.TripTimeTables.FindAsync(command.TripTimeTableId);

            if (trip is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["InvalidRequest"].Value);
            }

            var seat = trip.RecervedSeats.Where(x => x.setNum == command.SeatNum);

            _context.Remove(seat);
            _context.SaveChanges();

            _response.Message = _localization["SuccessProcess"].Value;

            return _response;
        }
        public async Task<BaseResponse> UpdateArriveTimeAsync(UpdateTripArriveTimeCommand command)
        {
            var trip = await _context.TripTimeTables.FindAsync(command.TripTimeTableId);

            if (trip is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["InvalidRequest"].Value);
            }

            trip.ArriveTime = command.ArriveTime;
            _context.Update(trip);
            _context.SaveChanges();

            _response.Message = _localization["SuccessProcess"].Value;
            return _response;
        }
        public async Task<BaseResponse> UpdateTripStatusAsync(UpdateOrgTripStatusCommand command)
        {
            var trip = await _context.TripTimeTables.FindAsync(command.TripTimeTableId);

            if (trip is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["InvalidRequest"].Value);
            }

            trip.Status = command.Status;

            _context.Update(trip);
            _context.SaveChanges();

            _response.Message = _localization["SuccessProcess"].Value;
            return _response;
        }
        public async Task<BaseResponse> GeAllReservationAsync(int tripTimeTableId)
        {
            var tripTimeTable = await _context.TripTimeTables.FindAsync(tripTimeTableId);

            if (tripTimeTable is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["InvalidRequest"].Value);
            }

            var reservations = tripTimeTable.Reservations.Select(x => new GetAllOrgTripReservationDto()
            {
                FullName = x.Customer.FirstName +' '+ x.Customer.LastName,
                UserName = x.Customer.UserName,
                IsRide = x.IsRide,
                PhotoUrl = x.Customer.PhotoUrl,
                LocationDescription = x.LocationDescription,
                OnRoad = x.OnRoad
            });

            _response.Data = reservations;
            return _response;
        }
        public async Task<BaseResponse> GetTripTimeTableLocationAsync(int tripTimeTableId)
        {
            var tripTimeTable = await _context.TripTimeTables.FindAsync(tripTimeTableId);

            if (tripTimeTable is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["InvalidRequest"].Value);
            }

            var location = new TripTimeTableLocationDto()
            {
                Start = _mapper.Map<TripTimeTableStationDto>(tripTimeTable.Trip.Line.Start),
                End = _mapper.Map<TripTimeTableStationDto>(tripTimeTable.Trip.Line.End)
            };

            _response.Data = location;
            return _response;
        }
        public async Task<BaseResponse> GetAccpetedPackagesRequestsAsync(string userId)
        {
            var trip = await _context.TripTimeTables.SingleOrDefaultAsync(x => x.DriverId == userId && x.StartTime >= DateTime.Now);

            var packages = await _context.Packages.Where(x => x.TripId == trip.Id)
                          .ProjectTo<AcceptedPackagesOrgDriver>(_mapper.ConfigurationProvider)
                          .ToListAsync();

            _response.Data = packages;
            return _response;
        }
        public async Task<BaseResponse> GetCurrentTrip(string userId)
        {
            var entity = await _context.TripTimeTables
                      .FirstOrDefaultAsync(x => x.DriverId == userId && x.StartTime <= DateTime.Now && x.ArriveTime >= DateTime.Now && x.Status != TripStatus.Arrived);

            var trip = _mapper.Map<CurrentOrganizationDriverTrip>(entity);

            _response.Data = trip;
            return _response;
        }

        public async Task<BaseResponse> UpdateCurrentOrgTripLocationAsync(string driverId, TripLocationUpdateDto tripDto)
        {
            DateTime currentData = DateTime.Now;
            var trip = await _context.TripTimeTables.
                FirstOrDefaultAsync(t => t.DriverId == driverId &&
                t.StartTime <= currentData &&
                (t.Status == TripStatus.OnRoad || t.Status == TripStatus.TakeBreak));
            if (trip == null)
                return BaseResponse.GetErrorException(HttpStatusErrorCode.NotFound, _localization["ObjectNotFound"].Value);
            trip.Langtitude = tripDto.Langtitude;
            trip.Latitude = tripDto.Latitude;
            _context.TripTimeTables.Update(trip);
            await _context.SaveChangesAsync();
            _response.Message = _localization["UpdateSuccess"].Value;
            return _response;
        }

        public async Task<BaseResponse> TakeBreakAsync(int id)
        {
            var tripCheck = await _context.TripTimeTables.FirstOrDefaultAsync(v => v.Id == id);
            if (tripCheck is null)
                return BaseResponse.GetErrorException(HttpStatusErrorCode.NotFound, _localization["tripNotFound"].Value);

            tripCheck.Status = TripStatus.TakeBreak;
            var result = _context.TripTimeTables.Update(tripCheck);
            await _context.SaveChangesAsync();
            _response.Message = _localization["updateTripSuccess"].Value;
            return _response;
        }
    }
}
