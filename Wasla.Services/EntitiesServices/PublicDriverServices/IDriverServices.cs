using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;

namespace Wasla.Services.EntitiesServices.PublicDriverServices
{
    public interface IDriverServices
    {
        Task<BaseResponse> GetPublicPackagesRequestAsync(string driverId);
        Task<BaseResponse> ReviewPackagesRequest(int packageId, int status);
        Task<BaseResponse> GetDriverPublicPackagesAsync(string DriverId);


    }
}
