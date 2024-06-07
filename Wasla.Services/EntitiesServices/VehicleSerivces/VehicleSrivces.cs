using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Services.EntitiesServices.OrganizationSerivces;

namespace Wasla.Services.EntitiesServices.VehicleSerivces
{
    public class VehicleSrivces:IVehicleSrivces
    {
        private readonly WaslaDb _dbContext;
        private readonly BaseResponse _response;
        private readonly IStringLocalizer<VehicleSrivces> _localization;
        private readonly IMapper _mapper;

        public VehicleSrivces(
            WaslaDb dbContext,
            IStringLocalizer<VehicleSrivces> localization,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _response = new();
            _localization = localization;
            _mapper = mapper;
        }

        public async Task<BaseResponse>GetVehicleById(int id)
        {
            var entity = await _dbContext.Vehicles.FindAsync(id);

            if (entity == null)
            {
                _response.IsSuccess = false;
                _response.Message = _localization["ObjectNotFound"].Value;
                return _response;
            }

            var vehicle = _mapper.Map<GetVehicleByIdDto>(entity);

            _response.Data = vehicle;
            return _response;
        }
    }
}
