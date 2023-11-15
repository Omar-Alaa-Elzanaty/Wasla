using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;

namespace Wasla.Services.AdminServices
{
	public interface IAdminService
	{
		Task<BaseResponse> DisplayOrganiztionRequestsAsync();
		Task<BaseResponse> ConfirmOrgnaizationRequestAsync(int requestId);
	}
}
