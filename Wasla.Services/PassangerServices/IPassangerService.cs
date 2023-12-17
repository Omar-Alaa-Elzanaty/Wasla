using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Services.PassangerServices
{
	public interface IPassangerService
	{
		Task<BaseResponse> ReservationAsync(List<int> SetsNum, int tripId, string custId);
		Task<BaseResponse> SetsRecordsAsync(int tripId);
		Task<BaseResponse> OrganizationRateAsync(OrganizationRate model);
		Task<BaseResponse> OrganizationRateRemoveAsync(string organizationId, string customerId);
	}
}
