using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
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
            newDriver.LicenseImageUrl = await _mediaSerivce.AddAsync(model.LicenseImageFile);
            newDriver.PhotoUrl = await _mediaSerivce.AddAsync(model.ImageFile);

            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _userManager.CreateAsync(newDriver, model.Password);
                    if (result is null || !result.Succeeded)
                    {
                        throw new BadRequestException(HelperServices.CollectIdentityResultErrors(result));
                    }
                    result = await _userManager.AddToRoleAsync(newDriver, Roles.Role_Driver);

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
        public async Task<BaseResponse> AddEmployeeAsync(EmployeeRegisterDto model, string? orgId)
        {
            Employee employee = _mapper.Map<Employee>(model);
            employee.OrgId = orgId;
            employee.UserName = model.Email.Split('@')[0].ToLower() + (model.NationalId % 10000).ToString() + '@' + "wasla.com";
            string password = string.Empty;
            var rand = new Random();

            for (int i = 0; i < 5; i++)
            {
                password += rand.Next(0, 9).ToString();
            }
            password += "@Wasla";

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
            _response.Data = station;
            return _response;
        }
        public async Task<BaseResponse> UpdateStationAsync(StationDto stationDto, string orgId)
        {
            var station = await _context.Stations.FirstOrDefaultAsync(v => v.Name == stationDto.Name && v.OrganizationId == orgId);

            if (station is null)
                throw new NotFoundException(_localization["StationNotFound"].Value);

            station.OrganizationId = orgId;
            station.Latitude = stationDto.Latitude;
            station.Langtitude = stationDto.Langtitude;
            station.Name = stationDto.Name;

            var result = _context.Stations.Update(station);
            await _context.SaveChangesAsync();

            _response.Data = result;

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
            return _response;
        }

        public async Task<BaseResponse> DeleteStationAsync(int id)
        {
            var station = await _context.Stations.FirstOrDefaultAsync(t => t.StationId == id);
            if (station == null)
                throw new NotFoundException(_localization["stationNotFound"].Value);
            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();
            _response.Data = station;
            return _response;
        }
        public async Task<BaseResponse> AddTripAsync(AddTripDto model, string orgId)
        {
            var trip = _mapper.Map<Trip>(model);
            trip.OrganizationId = orgId;

            await _context.Trips.AddAsync(trip);
            _ = await _context.SaveChangesAsync();

            _response.Data = trip;

            return _response;
        }
        public async Task<BaseResponse> UpdateTripAsync(UpdateTripDto model, int id)
        {
            var tripCheck = await _context.Trips.FirstOrDefaultAsync(v => v.Id == id);
            if (tripCheck is null)
                throw new NotFoundException(_localization["tripNotFound"].Value);
            //if (tripCheck.Driver != null)
            //{
            //    tripCheck.AvailablePackageSpace = tripCheck.Vehicle.PackageCapcity;
            //}
            //tripCheck.OrganizationId = model.orgId;
            //tripCheck.From = model.From;
            //tripCheck.To = model.To;
            //tripCheck.DriverId = model.DriverId;
            //tripCheck.Price = model.Price;
            //tripCheck.VehicleId = model.VehicleId;
            //tripCheck.Duration = model.arrivingTime > model.launchingTime ? model.arrivingTime - model.launchingTime : model.launchingTime - model.arrivingTime;

            var result = _context.Trips.Update(tripCheck);
            await _context.SaveChangesAsync();

            _response.Data = result;
            return _response;
        }
        public async Task<BaseResponse> GetTripsAsync(string orgId)
        {
            var trips = await _context.Trips.Where(t => t.OrganizationId == orgId).ToListAsync();
            var tripRes = _mapper.Map<List<TripDto>>(trips);
            _response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> GetTripAsync(int id)
        {
            var trip = await _context.Trips.Where(t => t.Id == id).FirstOrDefaultAsync();
            var tripRes = _mapper.Map<TripDto>(trip);

            _response.Data = tripRes;
            return _response;
        }

        public async Task<BaseResponse> GetTripsForDriverAsync(string orgId, string driverId)
        {
            //var trips = await _context.Trips.Where(t => t.OrganizationId == orgId && t.DriverId == driverId).Include(t => t.Vehicle).ToListAsync();
            //var tripRes = _mapper.Map<List<TripForDriverDto>>(trips);
            //_response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> DeleteTripAsync(int id)
        {
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.Id == id);
            if (trip == null)
                throw new NotFoundException(_localization["tripNotFound"].Value);
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            _response.Data = trip;
            return _response;
        }
        public async Task<BaseResponse> GetTripsForUserAsync(string orgId, string name)
        {
            //var trips = await _context.Trips.Where(t => t.OrganizationId == orgId && (t.From.StartsWith(name) || t.To.StartsWith(name))).ToListAsync();
            //var tripRes = _mapper.Map<List<TripForUserDto>>(trips);
            //_response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> GetTripsForUserWithToAndFromAsync(string orgId, string from, string to)
        {
            //var trips = await _context.Trips.Where(t => t.OrganizationId == orgId && (t.From == from || t.To == to)).ToListAsync();
            //var tripRes = _mapper.Map<List<TripForUserDto>>(trips);
            //_response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> GetOriganizationsWithName(string name)
        {
            var organizations = await _context.Organizations.Select(s => s.Name.StartsWith(name)).ToListAsync();
            _response.Data = _mapper.Map<ResponseOrgSearch>(organizations);
            return _response;
        }
    }
}
