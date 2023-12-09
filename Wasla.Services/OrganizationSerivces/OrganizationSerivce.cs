using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.Exceptions;
using Wasla.Services.MediaSerivces;

namespace Wasla.Services.OrganizationSerivces
{
	public class OrganizationSerivce:IOrganizationService
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

		public async Task<BaseResponse>AddVehicleAsync(VehicleDto vehicleModel,string orgId)
		{
			if(await _context.Vehicles.AnyAsync(v=>v.LicenseNumber == vehicleModel.LicenseNumber||v.LicenseWord==vehicleModel.LicenseWord))
			{
				throw new BadRequestException(_localization["VehicleExist"]);
			}

			Vehicle car=_mapper.Map<Vehicle>(vehicleModel);

			car.OrganizationId = orgId;
			if(vehicleModel.ImageFile is not null)
			{
				car.ImageUrl = await _mediaSerivce.AddAsync(vehicleModel.ImageFile);
			}
			_context.Add(car);
			_ = await _context.SaveChangesAsync();

			return _response;
		}
		public async Task<BaseResponse>UpdateVehicleAsync(VehicleDto model,int vehicleId)
		{
			var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);

			if(vehicle is null)
			{
				throw new KeynotFoundException(_localization["ObjectNotFound"].Value);
			}

			vehicle.Capcity = model.Capcity;
			vehicle.PackageCapcity=model.PackageCapcity;
			vehicle.Category = model.Category;
			vehicle.AdsSidesNumber = model.AdsSidesNumber;
			vehicle.LicenseNumber= model.LicenseNumber;
			vehicle.LicenseWord= model.LicenseWord;
			vehicle.Brand = model.Brand;

			if (model.ImageFile is not null)
			{
				await _mediaSerivce.RemoveAsync(vehicle.ImageUrl);
				vehicle.ImageUrl =await  _mediaSerivce.AddAsync(model.ImageFile); 
			}

			_context.Update(vehicle);
			_ = await _context.SaveChangesAsync();

			return _response;
		}
		public async Task<BaseResponse> DeleteVehicle(int vehicleId)
		{
			var vehicle = await _context.Vehicles.FindAsync(vehicleId);

			if(vehicle is null)
			{
				throw new BadRequestException(_localization["ObjectNotFound"].Value);
			}

			_context.Vehicles.Remove(vehicle);

			_response.Message = _localization["RemovedSuccessfully"].Value;
			return _response;
		}
		public async Task<BaseResponse>VehicleAnalysisAsync(string orgId)
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
			
			_response.Data=analysisResult;

			return _response;
		}
		public async Task<BaseResponse>AddDriver(OrgDriverDto model,string orgId)
		{
			if(orgId == null)
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
				Email=model.Email,
				FirstName=model.FirstName,
				LastName=model.LastName,
				Birthdate=model.BirthDate,
				Gender=model.Gender,
				LicenseNum=model.LicenseNumber,
				NationalId=model.NationalId,
				PhoneNumber = model.PhoneNumber,
				UserName=model.UserName
			};
			newDriver.LicenseImageUrl = await _mediaSerivce.AddAsync(model.LicenseImageFile);
			newDriver.PhotoUrl = await _mediaSerivce.AddAsync(model.ImageFile);

			using (var trans= await _context.Database.BeginTransactionAsync())
			{
				try
				{
					var result= await _userManager.CreateAsync(newDriver, model.Password);
					if(result is null || !result.Succeeded)
					{
						string errors = string.Empty;
						foreach(var error in result.Errors)
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
	} 
}
