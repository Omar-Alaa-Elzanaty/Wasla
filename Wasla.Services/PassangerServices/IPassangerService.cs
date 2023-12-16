using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;

namespace Wasla.Services.PassangerServices
{
	public interface IPassangerService
	{
		Task<BaseResponse> ReservationAsync(List<int> SetsNum, int tripId, string custId);
		Task<BaseResponse> SetsRecordsAsync(int tripId);
	}
}
