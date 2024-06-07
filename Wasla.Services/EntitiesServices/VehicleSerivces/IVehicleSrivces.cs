using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;

namespace Wasla.Services.EntitiesServices.VehicleSerivces
{
    public interface IVehicleSrivces
    {
        Task<BaseResponse> GetVehicleById(int id);
    }
}
