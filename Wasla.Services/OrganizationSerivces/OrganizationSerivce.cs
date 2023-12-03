using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
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
		public OrganizationSerivce(
			WaslaDb context,
			IStringLocalizer<OrganizationSerivce> localization,
			IMapper mapper,
			IMediaSerivce mediaSerivce)
		{
			_context = context;
			_response = new();
			_localization = localization;
			_mapper = mapper;
			_mediaSerivce = mediaSerivce;
		}

		public async Task<BaseResponse>AddVehicleAsync(AddVehicleDto vehicleModel,string orgId)
		{
			if(await _context.Vehicles.AnyAsync(v=>v.LicenseNumber == vehicleModel.LicenseNumber||v.LicenseWord==vehicleModel.LicenseWord))
			{
				throw new BadRequestException(_localization["VehicleExist"]);
			}

			Vehicle car=_mapper.Map<Vehicle>(vehicleModel);

			car.OrganizationId = orgId;
			car.ImageUrl = await _mediaSerivce.AddAsync(vehicleModel.ImageFile);

			_context.Add(car);
			_ = await _context.SaveChangesAsync();

			return _response;
		}
		public async Task<BaseResponse>VehicleAnalysisAsync(string orgId)
		{
			var analysisResult = _context.Vehicles.Where(v => v.OrganizationId == orgId).GroupBy(v => new
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
			});
			
			_response.Data=analysisResult;

			return _response;
		}
	}
}
