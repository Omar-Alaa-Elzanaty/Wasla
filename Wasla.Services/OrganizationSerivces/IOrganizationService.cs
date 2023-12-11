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
		Task<BaseResponse> AddVehicleAsync(VehicleDto vehicleModel, string orgId);
		Task<BaseResponse> VehicleAnalysisAsync(string orgId);
		Task<BaseResponse> UpdateVehicleAsync(VehicleDto model, int vehicleId);
		Task<BaseResponse> DeleteVehicleAsync(int vehicleId);
		Task<BaseResponse> AddDriverAsync(OrgDriverDto model, string orgId);
		Task<BaseResponse> AddEmployeeAsync(EmployeeRegisterDto model, string? orgId);
	}
}
