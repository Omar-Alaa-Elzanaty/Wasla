using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Globalization;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Helpers.Statics;
using Wasla.Model.Models;
using Wasla.Services.Exceptions;
using Wasla.Services.HlepServices.MediaSerivces;
using Wasla.Services.ShareService;

namespace Wasla.Services.EntitiesServices.OrganizationSerivces
{
    public class OrganizationSerivce : IOrganizationService
    {
        private readonly WaslaDb _context;
        private readonly BaseResponse _response;
        private readonly IStringLocalizer<OrganizationSerivce> _localization;
        private readonly IMapper _mapper;
        private readonly IMediaSerivce _mediaSerivce;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public OrganizationSerivce(
            WaslaDb context,
            IStringLocalizer<OrganizationSerivce> localization,
            IMapper mapper,
            IMediaSerivce mediaSerivce,
            UserManager<Account> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _response = new();
            _localization = localization;
            _mapper = mapper;
            _mediaSerivce = mediaSerivce;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<BaseResponse> DisplayVehicles(string orgId)
        {
            _response.Data = await _context.Vehicles.Where(v => v.OrganizationId == orgId).Select(v => new
            {
                v.Id,
                v.LicenseWord,
                v.LicenseNumber,
                v.ImageUrl
            }).ToListAsync();

            return _response;
        }
        public async Task<BaseResponse> AddVehicleAsync(VehicleDto vehicleModel, string orgId)
        {
            if (await _context.Vehicles.AnyAsync(v => v.LicenseNumber == vehicleModel.LicenseNumber || v.LicenseWord == vehicleModel.LicenseWord))
            {
                throw new BadRequestException(_localization["VehicleExist"]);
            }

            Vehicle car = _mapper.Map<Vehicle>(vehicleModel);

            car.OrganizationId = orgId;
            if (vehicleModel.ImageFile is not null)
            {
                car.ImageUrl = await _mediaSerivce.AddAsync(vehicleModel.ImageFile);
            }
            await _context.Vehicles.AddAsync(car);
            _ = await _context.SaveChangesAsync();

            return _response;
        }
        public async Task<BaseResponse> UpdateVehicleAsync(VehicleDto model, int vehicleId)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);

            if (vehicle is null)
            {
                throw new KeynotFoundException(_localization["ObjectNotFound"].Value);
            }

            if (await _context.Vehicles.AnyAsync(v => v.Id != vehicleId && v.LicenseNumber == model.LicenseNumber && v.LicenseWord == model.LicenseWord))
            {
                throw new BadRequestException(_localization["RepeatedData"].Value);
            }

            vehicle.Capcity = model.Capcity;
            vehicle.PackageCapcity = model.PackageCapcity;
            vehicle.Category = model.Category;
            vehicle.AdsSidesNumber = model.AdsSidesNumber;
            vehicle.LicenseNumber = model.LicenseNumber;
            vehicle.LicenseWord = model.LicenseWord;
            vehicle.Brand = model.Brand;

            if (model.ImageFile is not null)
            {
                vehicle.ImageUrl = await _mediaSerivce.UpdateAsync(vehicle.ImageUrl, model.ImageFile);
            }

            _context.Update(vehicle);
            _ = await _context.SaveChangesAsync();

            return _response;
        }
        public async Task<BaseResponse> DeleteVehicleAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);

            if (vehicle is null)
            {
                throw new BadRequestException(_localization["ObjectNotFound"].Value);
            }
            var imageUrl = vehicle.ImageUrl;

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            await _mediaSerivce.DeleteAsync(imageUrl);

            _response.Message = _localization["RemovedSuccessfully"].Value;
            return _response;
        }
        public async Task<BaseResponse> VehicleAnalysisAsync(string orgId)
        {
            var analysisResult = await _context.Vehicles.Where(v => v.OrganizationId == orgId).GroupBy(v => new
            {
                v.Brand,
                v.Capcity,
                v.Category
            }).Select(v => new
            {
                v.Key.Category,
                v.Key.Brand,
                v.Key.Capcity,
                TotalVehicles = v.Count()
            }).ToListAsync();

            _response.Data = analysisResult;

            return _response;
        }
        public async Task<BaseResponse> AddDriverAsync(OrgDriverDto model, string orgId)
        {
            if (await _context.Drivers.AnyAsync(od => (od.NationalId == model.NationalId
                                    || od.PhoneNumber == model.PhoneNumber
                                    || od.LicenseNum == model.LicenseNumber
                                    || model.Email != null && od.Email == model.Email) && od.OrganizationId == orgId))
            {
                throw new BadRequestException(_localization["AccountExist"].Value);
            }

            var newDriver = new Driver()
            {
                OrganizationId = orgId,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birthdate = model.BirthDate,
                Gender = model.Gender,
                LicenseNum = model.LicenseNumber,
                NationalId = model.NationalId,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName
            };
            newDriver.LicenseImageUrl =await _mediaSerivce.AddAsync(model.LicenseImageFile);
            newDriver.PhotoUrl =await _mediaSerivce.AddAsync(model.ImageFile);

            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _userManager.CreateAsync(newDriver, model.Password);
                    if (result is null || !result.Succeeded)
                    {
                        throw new BadRequestException(HelperServices.CollectIdentityResultErrors(result));
                    }
                    result = await _userManager.AddToRoleAsync(newDriver, Roles.Role_OrgDriver);

                    if (result is null || !result.Succeeded)
                    {
                        throw new BadRequestException(HelperServices.CollectIdentityResultErrors(result));
                    }

                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();

                    if (!newDriver.LicenseImageUrl.IsNullOrEmpty()) await _mediaSerivce.DeleteAsync(newDriver.LicenseImageUrl);

                    if (!newDriver.PhotoUrl.IsNullOrEmpty()) await _mediaSerivce.DeleteAsync(newDriver.PhotoUrl);
                }
            }
            return _response;
        }
        public async Task<BaseResponse> GetEmployees(string orgId)
        {
            var entities = await _context.Employees.Where(x => x.OrgId == orgId)
                            .ToListAsync();
            var employess = _mapper.Map<List<GetEmployeeOrganizationDto>>(entities);

            _response.Data = employess;
            return _response;
        }
        public async Task<BaseResponse> AddEmployeeAsync(EmployeeRegisterDto model, string? orgId)
        {
            Employee employee = _mapper.Map<Employee>(model);
            employee.OrgId = orgId;
            employee.UserName = model.Email.Split('@')[0].ToLower();// + (model.NationalId % 10000).ToString() + '@' + "wasla.com";
            string password = "123@Abc";// string.Empty;
            var rand = new Random();
            //
            //for (int i = 0; i < 5; i++)
            //{
            //    password += rand.Next(0, 9).ToString();
            //}
            //password += "@Wasla";

            if (model.PhotoFile is not null)
            {
                employee.PhotoUrl = await _mediaSerivce.AddAsync(model.PhotoFile);
            }

            _response.Message = _localization["RegisterSucccess"].Value;

            var result = await _userManager.CreateAsync(employee, password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + ", ";
                }
                if (employee.PhotoUrl is not null) await _mediaSerivce.DeleteAsync(employee.PhotoUrl);

                _response.Message = _localization["RegisterFail"];
                _response.IsSuccess = false;
            }

            var roleFound = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleFound)
            {
                throw new BadRequestException(_localization["roleNotFound"].Value);
            }

            await _userManager.AddToRoleAsync(employee, model.Role);

            return _response;
        }
        public async Task<BaseResponse> DeleteEmployeeAsync(string employeeId)
        {
            var user = _context.Employees.Find(employeeId); ;

            if (user is null)
            {
                throw new NotFoundException(_localization["UserNotFound"]);
            }

            if (user.PhotoUrl is not null)
            {
                await _mediaSerivce.DeleteAsync(user.PhotoUrl);
            }

            await _userManager.DeleteAsync(user);

            _response.Message = _localization["RemovedSuccessfully"].Value;

            return _response;
        }
        public async Task<BaseResponse> GetAllDrivers(string orgId)
        {
            var driver = await _context.Drivers.Where(d => d.OrganizationId == orgId).Select(d => new
            {
                d.Id,
                Name = d.FirstName + ' ' + d.LastName,
            }).ToListAsync();

            _response.Data = driver;

            return _response;
        }
        #region Ads
        public async Task<BaseResponse> AddAdsAsync(AdsDto model, string orgId)
        {
            var adsFound = await _context.Advertisments.AnyAsync(ads => ads.Name == model.Name);

            if (adsFound)
            {
                throw new BadRequestException(_localization["RepeatedName"].Value);
            }

            Advertisment newAds = _mapper.Map<Advertisment>(model);
            newAds.organizationId = orgId;
            newAds.ImageUrl = await _mediaSerivce.AddAsync(model.ImageFile);

            await _context.Advertisments.AddAsync(newAds);
            await _context.SaveChangesAsync();

            _response.Message = _localization["CreatedAdsSuccess"].Value;

            return _response;
        }
        public async Task<BaseResponse> AddAdsToVehicleAsync(int adsId, int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);

            var ads = await _context.Advertisments.FindAsync(adsId);

            if (vehicle is null || ads is null)
            {
                throw new BadRequestException(_localization["ObjectNotFound"].Value);
            }

            if (vehicle.Advertisment.Count() == vehicle.AdsSidesNumber)
            {
                throw new BadRequestException(_localization["ReachToLimit"].Value);
            }

            vehicle.Advertisment.Add(ads);

            _context.Update(vehicle);
            await _context.SaveChangesAsync();

            _response.Message = _localization["save"].Value;
            return _response;
        }
        public async Task<BaseResponse> RemoveAdsFromVehicleAsync(int adsId, int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);

            var ads = await _context.Advertisments.FindAsync(adsId);

            if (vehicle is null || ads is null)
            {
                throw new BadRequestException(_localization["ObjectNotFound"].Value);
            }
            vehicle.Advertisment.Remove(ads);

            _context.Update(vehicle);
            await _context.SaveChangesAsync();

            _response.Message = _localization["Removed"].Value;
            return _response;
        }
        public async Task<BaseResponse> UpdateAdsAsync(int adsId, AdsDto model)
        {
            var ads = await _context.Advertisments.FindAsync(adsId);

            if (ads is null)
            {
                throw new KeyNotFoundException(_localization["ObjectNotFound"].Value);
            }

            if (_context.Advertisments.Any(ad => ad.Name == model.Name && ad.Id != adsId))
            {
                throw new BadRequestException(_localization["RepeatedName"].Value);
            }

            ads.StartDate = model.StartDate;
            ads.EndDate = model.EndDate;
            ads.Name = model.Name;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (model.ImageFile is not null)
                    {
                        await _mediaSerivce.DeleteAsync(ads.ImageUrl);
                        ads.ImageUrl = await _mediaSerivce.UpdateAsync(ads.ImageUrl, model.ImageFile);
                    }
                    _context.Update(ads);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {

                    await transaction.RollbackAsync();
                }
            }

            _response.Message = _localization["SuccessProcess"].Value;
            return _response;
        }
        public async Task<BaseResponse> DeleteAdsAsync(int adsId)
        {
            var ads = await _context.Advertisments.FindAsync(adsId);

            if (ads is null)
            {
                throw new KeyNotFoundException(_localization["ObjectNotFound"].Value);
            }

            var imageUrl = ads.ImageUrl;
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Remove(ads);
                    await _context.SaveChangesAsync();
                    await _mediaSerivce.DeleteAsync(imageUrl);

                    await trans.CommitAsync();
                }
                catch
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }


            _response.Message = _localization["Removed"].Value;
            return _response;
        }
        #endregion

        #region Station
        public async Task<BaseResponse> AddStationAsync(StationDto stationDto, string orgId)
        {
            if (await _context.Stations.AnyAsync(v => v.Name == stationDto.Name && v.OrganizationId == orgId))
            {
                throw new BadRequestException(_localization["StationExist"].Value);
            }

            var station = _mapper.Map<Station>(stationDto);
            station.OrganizationId = orgId;

            await _context.Stations.AddAsync(station);
            _ = await _context.SaveChangesAsync();
            _response.Message = _localization["addStationSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> UpdateStationAsync(StationDto stationDto, string orgId,int stationId)
        {
            var station = await _context.Stations.FirstOrDefaultAsync(v => v.StationId == stationId && v.OrganizationId == orgId);

            if (station is null)
                throw new NotFoundException(_localization["StationNotFound"].Value);

         
            if(station.Name!=stationDto.Name)
            {
                if (await _context.Stations.AnyAsync(v => v.Name == stationDto.Name && v.OrganizationId == orgId))
                {
                    throw new BadRequestException(_localization["StationExist"].Value);
                }
            }
            station.Name = stationDto.Name;
            station.OrganizationId = orgId;
            station.Latitude = stationDto.Latitude;
            station.Langtitude = stationDto.Langtitude;
            var result = _context.Stations.Update(station);
            await _context.SaveChangesAsync();

            _response.Message = _localization["updateStationSuccess"].Value;

            return _response;
        }
        public async Task<BaseResponse> GetStationsAsync(string orgId)
        {
            var stations = await _context.Stations.Where(t => t.OrganizationId == orgId).ToListAsync();
            var tripRes = _mapper.Map<List<StationDto>>(stations);
            _response.Data = tripRes;
            return _response;
        }

        public async Task<BaseResponse> GetStationAsync(int id)
        {
            var station = await _context.Stations.FirstOrDefaultAsync(v => v.StationId == id);
            if (station is null)
                throw new NotFoundException(_localization["StationNotFound"].Value);
            var tripRes = _mapper.Map<StationDto>(station);
            _response.Data = tripRes;
            _response.Message = _localization["updateStationSuccess"].Value;

            return _response;
        }

        public async Task<BaseResponse> DeleteStationAsync(int id)
        {
            var station = await _context.Stations.FirstOrDefaultAsync(t => t.StationId == id);
            if (station == null)
                throw new NotFoundException(_localization["stationNotFound"].Value);
            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();
            _response.Message = _localization["deleteStationSuccess"].Value;
            return _response;
        }

        #endregion
        #region Line
        public async Task<BaseResponse> AddLineAsync(LineRequestDto lineDto)
        {
            if (await _context.Lines.AnyAsync(v => v.StartId == lineDto.StartId && v.EndId==lineDto.EndId))
            {
                throw new BadRequestException(_localization["LineExist"].Value);
            }

            var line = _mapper.Map<Line>(lineDto);
            await _context.Lines.AddAsync(line);
            _ = await _context.SaveChangesAsync();
            _response.Message = _localization["lineAddSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> UpdateLineAsync(LineRequestDto lineDto,int lineId)
        {
            var line = await _context.Lines.FirstOrDefaultAsync(v => v.Id == lineId);

            if (line is null)
                throw new NotFoundException(_localization["LineNotFound"].Value);
            if (line.StartId != lineDto.StartId && line.EndId != lineDto.EndId)
            {
                if (await _context.Lines.AnyAsync(v => v.StartId == lineDto.StartId && v.EndId == lineDto.EndId))
                {
                    throw new BadRequestException(_localization["LineExist"].Value);
                }
            }
            line.StartId = lineDto.StartId;
            line.EndId = lineDto.EndId;
            var result = _context.Lines.Update(line);
            await _context.SaveChangesAsync();
            //_response.Data = result;
            _response.Message = _localization["LineUpdateSuccess"];
            return _response;
        }
        public async Task<BaseResponse> GetLinessAsync(string orgId)
        {
            var lines = await _context.Lines.Where(t => t.Start.OrganizationId == orgId).ToListAsync();
            var tripRes = _mapper.Map<List<LineDto>>(lines);
            _response.Data = tripRes;
            return _response;
        }
       
        public async Task<BaseResponse> GetLineAsync(int id)
        {
            var line = await _context.Lines.FirstOrDefaultAsync(v => v.Id == id);
              if (line is null)
                throw new NotFoundException(_localization["LineNotFound"].Value);
            var tripRes = _mapper.Map<LineDto>(line);
           _response.Data = tripRes;
            return _response;
        }

        public async Task<BaseResponse> DeleteLineAsync(int id)
        {
            var line = await _context.Lines.FirstOrDefaultAsync(t => t.Id == id);
            if (line == null)
                throw new NotFoundException(_localization["LineNotFound"].Value);
            _context.Lines.Remove(line);
            await _context.SaveChangesAsync();
            _response.Message = _localization["linedeleteSuccess"].Value;
            return _response;
        }
        #endregion
        #region trip
        public async Task<BaseResponse> AddTripAsync(AddTripDto model, string orgId)
        {
            var tripExist= await _context.Trips.AnyAsync(t=>t.LineId==model.LineId&&t.OrganizationId==orgId);
            if (tripExist)
                throw new BadRequestException(_localization["TripAlreadyExist"].Value);
            var trip = _mapper.Map<Trip>(model);
           trip.Duration =TimeSpan.FromMinutes(model.Duration);
            trip.OrganizationId = orgId;

            await _context.Trips.AddAsync(trip);
            _ = await _context.SaveChangesAsync();
            _response.Message = _localization["addTripSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> UpdateTripAsync(UpdateTripDto model, int id)
        {
            var tripCheck = await _context.Trips.FirstOrDefaultAsync(v => v.Id == id);
            if (tripCheck is null)
                throw new NotFoundException(_localization["tripNotFound"].Value);
            tripCheck.AdsPrice = model.AdsPrice;
            tripCheck.IsPublic = model.IsPublic;
            tripCheck.Points = model.Points;
            tripCheck.LineId = model.LineId;
            tripCheck.Price = model.Price;
            tripCheck.Duration =TimeSpan.FromMinutes(model.Duration);
            var result = _context.Trips.Update(tripCheck);
            await _context.SaveChangesAsync();
            _response.Message = _localization["updateTripSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> GetTripsAsync(string orgId)
        {
            var trips = await _context.Trips.Where(t => t.OrganizationId == orgId).ToListAsync();
            var tripRes = _mapper.Map<List<TripDto>>(trips);
            _response.Data = tripRes;
            _response.Message = _localization["getTripSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> GetTripAsync(int id)
        {
            var trip = await _context.Trips.Where(t => t.Id == id).FirstOrDefaultAsync();
            var tripRes = _mapper.Map<TripDto>(trip);
            _response.Data = tripRes;
            _response.Message = _localization["getTripSuccess"].Value;

            return _response;
        }
        public async Task<BaseResponse> DeleteTripAsync(int id)
        {
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.Id == id);
            if (trip == null)
                throw new NotFoundException(_localization["tripNotFound"].Value);
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            _response.Message = _localization["deleteTripSuccess"].Value;
            return _response;
        }
        #endregion
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
        public async Task<BaseResponse> AddTripTimeAsync(AddTripTimeDto model)
        {
             var tripExist = await _context.TripTimeTables.AnyAsync(t => t.TripId == model.TripId &&
             t.StartTime == model.StartTime&&t.VehicleId==t.VehicleId&&t.DriverId==model.DriverId);
             if (tripExist)
                throw new BadRequestException(_localization["TripAlreadyExist"].Value);
              var trip = _mapper.Map<TripTimeTable>(model);
              await _context.TripTimeTables.AddAsync(trip);
             await _context.SaveChangesAsync();
            _response.Message = _localization["addTripSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> GetTripTimeAsync(int id)
        {
            var trip = await _context.TripTimeTables.Where(t => t.Id == id).FirstOrDefaultAsync();
            var tripRes = _mapper.Map<TripTimeDto>(trip);
          /*  tripRes.Points=trip.Trip.Points;
            tripRes.Price=trip.Trip.Price;
            tripRes.Line=trip.Trip.Line;
            tripRes.Duration=trip.Trip.Duration;*/
            _response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> GetTripsTimeAsync(string orgId)
        {
            var trips = await _context.TripTimeTables.Where(t => t.Trip.OrganizationId == orgId).ToListAsync();
            var tripRes = _mapper.Map<List<TripTimeDto>>(trips);
            _response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> UpdateTripTimeAsync(UpdateTripTimeDto model, int id)
        {
            var tripCheck = await _context.TripTimeTables.FirstOrDefaultAsync(v => v.Id == id);
            if (tripCheck is null)
                throw new NotFoundException(_localization["tripNotFound"].Value);
            var tripExist = await _context.TripTimeTables.AnyAsync(t => t.TripId == model.TripId &&
                        t.StartTime == model.StartTime && t.VehicleId == t.VehicleId && t.DriverId == model.DriverId&&t.Id!=id);
            if (tripExist)
                throw new BadRequestException(_localization["TripAlreadyExist"].Value);
            tripCheck.TripId = model.TripId;
            tripCheck.StartTime=model.StartTime;
            tripCheck.ArriveTime = model.ArriveTime;
            tripCheck.IsStart = model.IsStart;
            tripCheck.VehicleId = model.VehicleId;
            tripCheck.AvailablePackageSpace = model.AvailablePackageSpace;
            var result = _context.TripTimeTables.Update(tripCheck);
            await _context.SaveChangesAsync();
            _response.Message = _localization["updateTripSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> TakeBreakAsync(int id)
        {
            var tripCheck = await _context.TripTimeTables.FirstOrDefaultAsync(v => v.Id == id);
            if (tripCheck is null)
                return BaseResponse.GetErrorException(HttpStatusErrorCode.NotFound,_localization["tripNotFound"].Value);
          
            tripCheck.Status = TripStatus.TakeBreak;
            var result = _context.TripTimeTables.Update(tripCheck);
            await _context.SaveChangesAsync();
            _response.Message = _localization["updateTripSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> DeleteTripTimeAsync(int id)
        {
            var trip = await _context.TripTimeTables.FirstOrDefaultAsync(t => t.Id == id);
            if (trip == null)
                throw new NotFoundException(_localization["TripNotFound"].Value);
            _context.TripTimeTables.Remove(trip);
            await _context.SaveChangesAsync();
            _response.Data = trip;
            return _response;
        }
        public async Task<BaseResponse> GetTripsForDriverAsync(string orgId, string driverId)
        {
            var trips = await _context.TripTimeTables.Where(t => t.Trip.OrganizationId == orgId && t.DriverId == driverId).ToListAsync();
            TripForDriverDto tripDriver= new TripForDriverDto();
            var tripRes = _mapper.Map<List<TripForDriverDto>>(trips);

            _response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> GetTripsForDriverForNext7DaysAsync(TripForDriverRequestDto tripRequest)
        {
            DateTime endDate = tripRequest.CurrentDate.AddDays(7);


            var trips = await _context.TripTimeTables.
                Where(t => t.Trip.OrganizationId == tripRequest.OrgId &&
                t.DriverId == tripRequest.DriverId &&
                t.StartTime >= tripRequest.CurrentDate && t.StartTime <= endDate &&
                t.Status != TripStatus.Arrived && t.Status != TripStatus.OnRoad).OrderBy(t => t.StartTime)
                .Select(t => new TripForOrgDriverDays
                {
                    TripTimeTableId = t.Id,
                    TripDate = t.StartTime.ToString("MM/dd/yyyy"),
                    TripDay = t.StartTime.ToString("dddd"),
                    TripStartTime = t.StartTime.ToString("h:mm tt"),
                    StartStation = t.Trip.Line.Start.Name,
                    EndStation=t.Trip.Line.End.Name

                }).ToListAsync();
          //  TripForDriverDto tripDriver = new TripForDriverDto();
           // var tripRes = _mapper.Map<List<TripForDriverDto>>(trips);

            _response.Data = trips;
            return _response;
        }

        public async Task<BaseResponse> GetTripsHistoryForDriverAsync(string orgId, string DriverId, DateTime currentDate)
        {

            var trips = await _context.TripTimeTables.
                Where(t => t.Trip.OrganizationId ==orgId &&
                t.DriverId == DriverId &&
                 t.StartTime < currentDate &&
               t.Status == TripStatus.Arrived).OrderBy(t => t.StartTime)
                .Select(t => new TripForOrgDriverDays
                {
                    TripTimeTableId = t.Id,
                    TripDate = t.StartTime.ToString("MM/dd/yyyy"),
                    TripDay = t.StartTime.ToString("dddd"),
                    TripStartTime = t.StartTime.ToString("h:mm tt"),
                    StartStation = t.Trip.Line.Start.Name,
                    EndStation = t.Trip.Line.End.Name

                }).ToListAsync();
            _response.Data = trips;
            return _response;
        }

        public async Task<BaseResponse> GetTripsForUserAsync(string orgId, string lineName)
        {
             var trips = await _context.TripTimeTables.Where(t => t.Trip.OrganizationId == orgId && (t.Trip.Line.Start.Name.StartsWith(lineName) || t.Trip.Line.End.Name.StartsWith(lineName))).ToListAsync();
             var tripRes = _mapper.Map<List<TripForUserDto>>(trips);
             _response.Data = tripRes;
             return _response;
        }
        public async Task<BaseResponse> GetTripsForUserWithToAndFromAsync(string orgId, string from, string to)
        {
            var trips = await _context.TripTimeTables.Where(t => t.Trip.OrganizationId == orgId && (t.Trip.Line.Start.Name == from || t.Trip.Line.End.Name == to)).ToListAsync();
            var tripRes = _mapper.Map<List<TripForUserDto>>(trips);
            _response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> GetTripsByLineIdAsync(string orgId,int lineId)
        {
            var trips = await _context.TripTimeTables.Where(t => t.Trip.OrganizationId == orgId && (t.Trip.Line.Id==lineId)).ToListAsync();
            var tripRes = _mapper.Map<List<TripForUserDto>>(trips);
            _response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> GetTripsByLineIdInPublicAsync(int lineId)
        {
            var trips = await _context.TripTimeTables.Where(t =>t.Trip.Line.Id == lineId).ToListAsync();
            var tripRes = _mapper.Map<List<TripForUserDto>>(trips);
            _response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> GetOriganizationsWithName(string name)
        {
            var organizations = await _context.Organizations.Select(s => s.Name.StartsWith(name)).ToListAsync();
            _response.Data = _mapper.Map<ResponseOrgSearch>(organizations);
            return _response;
        }
        public async Task<BaseResponse> GetPackagesTripAsync(int tripId)
        {
            var package =await _context.Packages.Where(p => p.TripId == tripId).ToListAsync();
            var resault = _mapper.Map<List<OrgPackagesDto>>(package);
            _response.Data = resault;
            _response.Message = _localization["getPackageSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> GetPackageAsync(int packageId)
        {
            var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == packageId);
            if (package is null)
            {
                throw new KeynotFoundException(_localization["ObjectNotFound"].Value);
            }
            if (package.DriverId!=null)
            {
                _response.Data = _mapper.Map<PublicPackagesDto>(package);
            }
            else
            {
                _response.Data = _mapper.Map<OrgPackagesDto>(package);
            }
            _response.Message = _localization["getPackageSuccess"].Value;
          return _response;
        }
        public async Task<BaseResponse> GetPackagesRequestAsync(string orgId)
        {
           
            var packages = await _context.Packages.Where(p => p.TripId!=null&&p.Trip.Trip.OrganizationId==orgId&&p.Status==0).ToListAsync();
            var res = _mapper.Map<OrgPackagesDto>(packages);
            _response.Data = res;
            return _response;
        }
        public async Task<BaseResponse> ReviewPackagesRequest(int packageId,int status)
        {
            var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == packageId);

            if (package is null)
            {
                throw new KeynotFoundException(_localization["ObjectNotFound"].Value);
            }
              package.Status = status;
            await _context.SaveChangesAsync();
            _response.Message = _localization["PackageReviewedSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> GetTripsTimeByTripIdAndDate(int tripId, string date)
        {
           
            if (DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var searchDate))
            {
                var trips = await _context.TripTimeTables.Where(t => t.TripId == tripId &&t.StartTime.Date==searchDate ).ToListAsync();
                var tripRes = _mapper.Map<List<TripForUserDto>>(trips);
                _response.Data = tripRes;
            }
          return _response;
        }

        public async Task<BaseResponse> GetNextTripForDriver(string orgId, string DriverId,DateTime currentDate)
        {
            var trip = await _context.TripTimeTables.
                            Where(t => t.Trip.OrganizationId == orgId &&
                            t.DriverId == DriverId &&
                            t.StartTime >= currentDate &&
                            t.Status != TripStatus.Arrived).OrderBy(t => t.StartTime).FirstOrDefaultAsync();
            if (trip != null)
            {
                var response = new TripForOrgDriverDays
                {
                    TripTimeTableId = trip.Id,
                    TripDate = trip.StartTime.ToString("MM/dd/yyyy"),
                    TripDay = trip.StartTime.ToString("dddd"),
                    TripStartTime = trip.StartTime.ToString("h:mm tt"),
                    StartStation = trip.Trip.Line.Start.Name,
                    EndStation = trip.Trip.Line.End.Name
                };
                _response.Data = response;
            }
            else { _response.Message = _localization["NoTripForDriver"].Value; }
            return _response;
        }

        public async Task<BaseResponse> IncreaseTripSeats(AddTripSeatDto tripSeat)
        {
            _context.Seats.Add(new Seat() { setNum =tripSeat.seatNumber, TripTimeTableId =tripSeat.tripId });
            await _context.SaveChangesAsync();
            return _response;
            
        }

        public async Task<BaseResponse> GetAllAds(string orgId)
        {
            var entities = await _context.Advertisments.Where(x => x.organizationId == orgId)
                            .ToListAsync();

            var advertisments = _mapper.Map<List<GetOrganizationAds>>(entities);

            _response.Data = advertisments;
            return _response;
        }
    }
}
