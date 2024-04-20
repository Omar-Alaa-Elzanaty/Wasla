using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Statics;
using Wasla.Model.Models;
using Wasla.Services.Authentication.AuthHelperService.FactorService.Factory;
using Wasla.Services.Exceptions;
using Wasla.Services.HlepServices.MediaSerivces;
using Wasla.Services.ShareService;
using Wasla.Services.ShareService.EmailServices;

namespace Wasla.Services.Authentication.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly WaslaDb _context;
        private readonly BaseResponse _response;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        private readonly IStringLocalizer<AdminService> _localization;
        private readonly IMailServices _mailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMediaSerivce _mediaService;

        public AdminService(
            WaslaDb dbContext,
            IMapper mapper,
            UserManager<Account> userManager,
            IStringLocalizer<AdminService> stringLocalizer,
            IMailServices mailService, RoleManager<IdentityRole> roleManager,
            IMediaSerivce mediaService)
        {
            _response = new();
            _context = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _localization = stringLocalizer;
            _mailService = mailService;
            _roleManager = roleManager;
            _mediaService = mediaService;
        }
        public async Task<BaseResponse> DisplayOrganiztionRequestsAsync()
        {
            var requests = await _context.OrganizationsRegisters.ToListAsync();

            if (requests is null || requests.Count == 0)
            {
                throw new NotFoundException(_localization["InvalidRequest"].Value);
            }

            _response.Data = requests;
            return _response;
        }
        public async Task<BaseResponse> ConfirmOrgnaizationRequestAsync(int requestId)
        {
            var request = await _context.OrganizationsRegisters.SingleOrDefaultAsync(r => r.RequestId == requestId);

            if (request is null)
            {
                throw new KeyNotFoundException(_localization["InvalidRequest"].Value);
            }

            var organization = _mapper.Map<Organization>(request);
            organization.UserName = request.Email;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _userManager.CreateAsync(organization, request.Password);
                    if (!result.Succeeded)
                    {
                        var errors = HelperServices.CollectIdentityResultErrors(result);

                        throw new BadRequestException(errors);
                    }
                    var role = "Org_" + organization.UserName.Split('@')[0] + "_SuperAdmin";
                    var newRole = new IdentityRole(role);
                    var roleRes = await _roleManager.CreateAsync(newRole);
                    var roleResult = await _userManager.AddToRoleAsync(organization, role);

                    if (!roleResult.Succeeded)
                    {
                        throw new ServerErrorException(_localization["RegisterFaild"].Value);
                    }

                    var roleClaim = await _roleManager.FindByNameAsync(role);

                    var permissions = OrgPermissions.GenerateAllPermissions();
                    foreach (var permission in permissions)
                        await _roleManager.AddClaimAsync(roleClaim, new Claim(PermissionsName.Org_Permission, permission));
                    //TODO: remove comment
                    /*await _mailService.SendEmailAsync(
						  mailTo: request.Email,
						  subject: "Wasla Email Annoucment",
						  body: "Your email has been activated,now you can login using your username and password");*/

                    _context.OrganizationsRegisters.Remove(request);
                    await transaction.CommitAsync();

                    _response.Message = $"{request.Name} account confirmed";
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }

            await _context.SaveChangesAsync();
            return _response;
        }
        public async Task<BaseResponse> CancelOrganizationRequestAsync(int requestId)
        {
            var request = await _context.OrganizationsRegisters.FirstOrDefaultAsync(i => i.RequestId == requestId);

            if (request is null)
            {
                throw new BadRequestException(_localization["InvalidRequest"].Value);
            }

            _context.OrganizationsRegisters.Remove(request);
            return _response;
        }
        public async Task<BaseResponse> DisplayOrganizationRequestAsync()
        {
            var entities = await _context.OrganizationsRegisters.ToListAsync();

            var requests = _mapper.Map<List<DisplayOrganizationRequest>>(entities);

            _response.Data = entities;

            return _response;
        }

        public async Task<BaseResponse> GetAllOrgsAsync()
        {
            var orgs= await _context.Organizations.ToListAsync();
            var orgsRes = _mapper.Map<List<OrganizationDto>>(orgs);
            _response.Data = orgsRes;
            return _response;
        }

        #region Station
        public async Task<BaseResponse> AddStationAsync(StationDto stationDto)
        {
            if (await _context.PublicStations.AnyAsync(v => v.Name == stationDto.Name))
            {
                throw new BadRequestException(_localization["StationExist"].Value);
            }

            var station = _mapper.Map<PublicStation>(stationDto);
            await _context.PublicStations.AddAsync(station);
            _ = await _context.SaveChangesAsync();
            _response.Message = _localization["addStationSuccess"].Value;
            // _response.Data = station;
            return _response;
        }
        public async Task<BaseResponse> UpdateStationAsync(StationDto stationDto, int stationId)
        {
            var station = await _context.PublicStations.FirstOrDefaultAsync(v => v.StationId == stationId);

            if (station is null)
                throw new NotFoundException(_localization["StationNotFound"].Value);


            if (station.Name != stationDto.Name&&( await _context.Stations.AnyAsync(v => v.Name == stationDto.Name)))
            {
              throw new BadRequestException(_localization["StationExist"].Value);
            }
            station.Name = stationDto.Name;
            station.Latitude = stationDto.Latitude;
            station.Langtitude = stationDto.Langtitude;
            var result = _context.PublicStations.Update(station);
            await _context.SaveChangesAsync();
            _response.Message = _localization["updateStationSuccess"].Value;

            return _response;
        }
        public async Task<BaseResponse> GetStationsAsync()
        {
            var stations = await _context.PublicStations.ToListAsync();
            var tripRes = _mapper.Map<List<StationDto>>(stations);
            _response.Data = tripRes;
            return _response;
        }

        public async Task<BaseResponse> GetStationAsync(int id)
        {
            var station = await _context.PublicStations.FirstOrDefaultAsync(v => v.StationId == id);
            if (station is null)
                throw new NotFoundException(_localization["StationNotFound"].Value);
            var tripRes = _mapper.Map<StationDto>(station);
            _response.Data = tripRes;
            return _response;
        }

        public async Task<BaseResponse> DeleteStationAsync(int id)
        {
            var station = await _context.PublicStations.FirstOrDefaultAsync(t => t.StationId == id);
            if (station == null)
                throw new NotFoundException(_localization["stationNotFound"].Value);
            _context.PublicStations.Remove(station);
            await _context.SaveChangesAsync();
            _response.Message = _localization["deleteStationSuccess"].Value;
            return _response;
        }
        #endregion
        public async Task<BaseResponse> GetActiveDrivers(string startStation, string endStation)
        {
            var drivers=await _context.PublicDriverTrips.
                Where(d=>d.StartStation.Name== startStation && d.EndStation.Name == endStation && d.IsActive == true)
                .Select(t => new ActivepublicDriverDto{
                    DriverName=t.PublicDriver.FirstName,
                    EndStation=t.EndStation.Name,
                    StartStation=t.StartStation.Name,
                    AcceptPackages=t.AcceptPackages,
                    AcceptReservationRequest=t.AcceptRequests


            }).ToListAsync();
            _response.Data = drivers;
            return _response;
        }
    }
}
