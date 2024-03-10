using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.EntitiesServices.OrganizationSerivces;
using Wasla.Services.Exceptions;
using Wasla.Services.HlepServices.MediaSerivces;

namespace Wasla.Services.EntitiesServices.PublicDriverServices
{
    public class DriverServices:IDriverServices
    {
        private readonly WaslaDb _context;
        private readonly BaseResponse _response;
        private readonly IStringLocalizer<OrganizationSerivce> _localization;
        private readonly IMapper _mapper;
        public DriverServices(
            WaslaDb context,
            IStringLocalizer<OrganizationSerivce> localization,
            IMapper mapper)
        {
            _context = context;
            _response = new();
            _localization = localization;
            _mapper = mapper;
        }
        public async Task<BaseResponse> GetPublicPackagesRequestAsync(string driverId)
        {
            var packages = await _context.Packages.Where(p => p.DriverId != null && p.DriverId == driverId && p.Status == 0).ToListAsync();
            var res = _mapper.Map<DriverPackagesDto>(packages);
            _response.Data = res;
            return _response;
        }

        public async Task<BaseResponse> ReviewPackagesRequest(int packageId, int status)
        {
            var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == packageId);

            if (package is null)
            {
                throw new KeynotFoundException(_localization["ObjectNotFound"].Value);
            }
            package.Status = status;
            await _context.SaveChangesAsync();
            _response.Message = _localization["PackageReviewedSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> GetDriverPublicPackagesAsync(string DriverId)
        {
            var packages = await _context.Packages.Where(p => p.DriverId == DriverId).ToListAsync();
            var res = _mapper.Map<DriverPackagesDto>(packages);
            _response.Data = res;
            return _response;
        }
    }
}
