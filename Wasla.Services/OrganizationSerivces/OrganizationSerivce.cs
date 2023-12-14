using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.Exceptions;
using Wasla.Services.MediaSerivces;

namespace Wasla.Services.OrganizationSerivces
{
	public class OrganizationSerivce : IOrganizationService
	{
		private readonly WaslaDb _context;
		private readonly BaseResponse _response;
		private readonly IStringLocalizer<OrganizationSerivce> _localization;
		private readonly IMapper _mapper;
		private readonly IMediaSerivce _mediaSerivce;
		private readonly UserManager<Account> _userManager;
		public OrganizationSerivce(
			WaslaDb context,
			IStringLocalizer<OrganizationSerivce> localization,
			IMapper mapper,
			IMediaSerivce mediaSerivce,
			UserManager<Account> userManager)
		{
			_context = context;
			_response = new();
			_localization = localization;
			_mapper = mapper;
			_mediaSerivce = mediaSerivce;
			_userManager = userManager;
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
			_context.Add(car);
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

			vehicle.Capcity = model.Capcity;
			vehicle.PackageCapcity = model.PackageCapcity;
			vehicle.Category = model.Category;
			vehicle.AdsSidesNumber = model.AdsSidesNumber;
			vehicle.LicenseNumber = model.LicenseNumber;
			vehicle.LicenseWord = model.LicenseWord;
			vehicle.Brand = model.Brand;

			if (model.ImageFile is not null)
			{
				await _mediaSerivce.RemoveAsync(vehicle.ImageUrl);
				vehicle.ImageUrl = await _mediaSerivce.AddAsync(model.ImageFile);
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

			_context.Vehicles.Remove(vehicle);

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
			if (orgId == null)
			{
				throw new ArgumentNullException("Organization Id");
			}

			if (await _context.Drivers.AnyAsync(od => (od.NationalId == model.NationalId
									|| od.PhoneNumber == model.PhoneNumber
									|| od.LicenseNum == model.LicenseNumber
									|| (model.Email != null && od.Email == model.Email)) && od.OrganizationId == orgId))
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
						string errors = string.Empty;
						foreach (var error in result.Errors)
						{
							errors += error + ", ";
						}
						throw new BadRequestException(errors);
					}
					await trans.CommitAsync();
				}
				catch (Exception)
				{
					await trans.RollbackAsync();

					if (!newDriver.LicenseImageUrl.IsNullOrEmpty()) await _mediaSerivce.RemoveAsync(newDriver.LicenseImageUrl);

					if (!newDriver.PhotoUrl.IsNullOrEmpty()) await _mediaSerivce.RemoveAsync(newDriver.PhotoUrl);
				}
			}
			return _response;
		}
		public async Task<BaseResponse> AddEmployeeAsync(EmployeeRegisterDto model,string? orgId)
		{
			if(orgId is null)
			{
				throw new ArgumentNullException(nameof(orgId));
			}

			Employee employee = _mapper.Map<Employee>(model);
			employee.OrgId = orgId;
			string password = string.Empty;
			employee.UserName = model.Email.Split('@')[0].ToLower() + (model.NationalId % 10000).ToString() + '@' + "wasla.com";
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
				if (employee.PhotoUrl is not null) await _mediaSerivce.RemoveAsync(employee.PhotoUrl);

				_response.Message = _localization["RegisterFail"];
			}

			return _response;
		}
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
			station.Name=stationDto.Name;
            var result=_context.Stations.Update(station);
            await  _context.SaveChangesAsync();
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
            var station = await _context.Stations.FirstOrDefaultAsync(v => v.StationId==id);
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
            var tripCheck = await _context.Trips.FirstOrDefaultAsync(v => v.Id==id);
            if (tripCheck is null)
              throw new NotFoundException(_localization["tripNotFound"].Value);
			if(tripCheck.Driver!=null)
			{
               tripCheck.AvailablePackageSpace = tripCheck.Vehicle.PackageCapcity;
               tripCheck.AvailableSets = tripCheck.Vehicle.Capcity;
            }
            tripCheck.OrganizationId = model.orgId;
            tripCheck.From = model.From;
            tripCheck.To=model.To;
            tripCheck.DriverId = model.DriverId;
            tripCheck.Price = model.Price;
            tripCheck.VehicleId = model.VehicleId;
            tripCheck.Duration = (model.arrivingTime > model.launchingTime) ? (model.arrivingTime - model.launchingTime) : (model.launchingTime - model.arrivingTime);

            var result = _context.Trips.Update(tripCheck);
			await _context.SaveChangesAsync();
		
            _response.Data = result;
            return _response;
        }
		public async Task<BaseResponse> GetTripsAsync(string orgId)
		{
			var trips = await _context.Trips.Where(t => t.OrganizationId == orgId).Include(t=>t.Driver).Include(t=>t.Vehicle).ToListAsync();
            var tripRes = _mapper.Map<List<TripDto>>(trips);
            _response.Data=tripRes;
			return _response;
        }
        public async Task<BaseResponse> GetTripAsync(int id)
        {
            var trip = await _context.Trips.Where(t => t.Id == id).Include(t => t.Driver).Include(t => t.Vehicle).FirstOrDefaultAsync();
			var tripRes = _mapper.Map<TripDto>(trip);
            _response.Data = tripRes;
            return _response;
        }
       
        public async Task<BaseResponse> GetTripsForDriverAsync(string orgId,string driverId)
        {
            var trips = await _context.Trips.Where(t => t.OrganizationId == orgId&&t.DriverId==driverId).Include(t => t.Vehicle).ToListAsync();
			var tripRes=_mapper.Map<List<TripForDriverDto>>(trips);
            _response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> DeleteTripAsync(int id)
        {
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.Id == id);
            if (trip==null)
                throw new NotFoundException(_localization["tripNotFound"].Value);
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            _response.Data = trip;
            return _response;
        }
		public async Task<BaseResponse> GetTripsForUserAsync(string orgId,string name)
		{
            var trips = await _context.Trips.Where(t => t.OrganizationId == orgId && (t.From.StartsWith(name)||t.To.StartsWith(name))).ToListAsync();
            var tripRes = _mapper.Map<List<TripForUserDto>>(trips);
            _response.Data = tripRes;
            return _response;
        }
        public async Task<BaseResponse> GetTripsForUserWithToAndFromAsync(string orgId, string from,string to)
        {
            var trips = await _context.Trips.Where(t => t.OrganizationId == orgId && (t.From==from||t.To==to)).ToListAsync();
            var tripRes = _mapper.Map<List<TripForUserDto>>(trips);
            _response.Data = tripRes;
            return _response;
        }


    }
}
