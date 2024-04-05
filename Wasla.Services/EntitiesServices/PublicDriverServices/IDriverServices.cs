using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Enums;

namespace Wasla.Services.EntitiesServices.PublicDriverServices
{
    public interface IDriverServices
    {
        Task<BaseResponse> GetPublicPackagesRequestAsync(string driverId);
        Task<BaseResponse> ReviewPackagesRequest(int packageId, int status);
        Task<BaseResponse> GetDriverPublicPackagesAsync(string DriverId);
        Task<BaseResponse> GetProfileAsync(string userId);
        Task<BaseResponse> GetTripStatus(string userId);
        Task<BaseResponse> UpdateTripStatus(int tripId, PublicTripSatus status);

    }
}
