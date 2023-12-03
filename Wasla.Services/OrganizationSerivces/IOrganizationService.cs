﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;

namespace Wasla.Services.OrganizationSerivces
{
	public interface IOrganizationService
	{
		Task<BaseResponse> AddVehicleAsync(AddVehicleDto vehicleModel, string orgId);
		Task<BaseResponse> VehicleAnalysisAsync(string orgId);
	}
}