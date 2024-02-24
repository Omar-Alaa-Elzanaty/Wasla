using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;
using Wasla.Services.Exceptions;
using Wasla.Services.HlepServices.MediaSerivces;

namespace Wasla.Services.EntitiesServices.PassangerServices
{
    public class PassangerService : IPassangerService
    {
        private readonly WaslaDb _context;
        private readonly BaseResponse _response;
        private readonly IMediaSerivce _mediaSerivce;
        private readonly IStringLocalizer<PassangerService> _localization;
        private readonly IMapper _mapper;
        public PassangerService
            (WaslaDb context,
            IStringLocalizer<PassangerService> localization,
            IMediaSerivce mediaSerivce,IMapper mapper)
        {
            _context = context;
            _response = new();
            _localization = localization;
            _mediaSerivce = mediaSerivce;
            _mapper = mapper;
        }

        public async Task<BaseResponse> SeatsRecordsAsync(int tripId)
        {
            var trip = await _context.Trips.FindAsync(tripId);

            if (trip is null)
            {
                throw new KeyNotFoundException(_localization["ObjectNotFound"].Value);
            }

            //var reciveredSets = trip.RecervedSets.Select(s => s.setNum).ToHashSet();

            var sets = new List<SetStatusDto>();

            //for (int setNum = 1; setNum <= trip.Capacity; setNum++)
            //{
            //    sets.Add(new()
            //    {
            //        SetNum = setNum,
            //        ISAvailable = !reciveredSets.Contains(setNum)
            //    });
            //}

            _response.Data = sets;

            return _response;

        }
        public async Task<BaseResponse> ReservationAsync(ReservationDto order)
        {
            List<Reservation> completeReserve = new List<Reservation>();

            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var set in order.seats)
                    {
                        _context.Seats.Add(new Seat() { setNum = set.SeatNum, TripId = order.TripId });
                        _context.SaveChanges();
                        completeReserve.Add(new Reservation()
                        {
                            SetNum = set.SeatNum,
                            CustomerId = order.CustomerId,
                            ReservationDate = DateTime.Now,
                            TripId = order.TripId,
                            QrCodeUrl = " "//await _mediaSerivce.AddAsync(set.QrCodeFile)
                        });

                    }
                    await _context.Reservations.AddRangeAsync(completeReserve);

                    var customer = await _context.Customers.FindAsync(order.CustomerId);
                    var trip = await _context.Trips.FindAsync(order.TripId);

                    if (customer is null || trip is null)
                    {
                        throw new KeynotFoundException(_localization["ReservationFail"].Value);
                    }

                    customer.points += completeReserve.Count * trip.Points;

                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (KeynotFoundException)
                {
                    await trans.RollbackAsync();
                    throw;
                }
                catch
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }

            _response.Message = _localization["SuccessProcess"].Value;
            _response.Data = completeReserve.Select(t => new
            {
                t.Id,
                t.SetNum,
                t.TripId,
                t.QrCodeUrl,
                t.ReservationDate
            });

            return _response;
        }
        public async Task<BaseResponse> OrganizationRateAsync(OrganizationRate model)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == model.CustomerId);
            var rate = customer?.OrganizationRates.FirstOrDefault(r => r.OrgId == model.OrgId);

            if (customer is not null && rate is null)
            {
                customer.OrganizationRates.Add(model);

                _response.Message = _localization["AddRate"].Value;
            }
            else if (customer is not null && rate is not null)
            {
                rate.OrgId = model.OrgId;
                rate.CustomerId = model.CustomerId;
                rate.Rate = model.Rate;

                _response.Message = _localization["UpdateRate"].Value;
            }

            await _context.SaveChangesAsync();

            return _response;
        }
        public async Task<BaseResponse> OrganizationRateRemoveAsync(string organizationId, string customerId)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == customerId);
            var rate = customer?.OrganizationRates
                    .SingleOrDefault(r => r.OrgId == organizationId);

            if (customer is null || rate is null)
            {
                throw new KeynotFoundException(_localization["ObjectNotFound"].Value);
            }

            customer.OrganizationRates.Remove(rate);
            await _context.SaveChangesAsync();

            _response.Message = _localization["RemoveRate"].Value;

            return _response;
        }
        public async Task<BaseResponse> GetLinesAsync(string orgId)
        {
            var lines = await _context.Lines.Where(t => t.Start.OrganizationId == orgId).ToListAsync();
            var lineRes = _mapper.Map<List<LinePackagesdto>>(lines);
            var lineDtos = new List<LinePackagesdto>();
            var linRev = new LinePackagesdto();
            foreach (var lineDto in lineRes)
            {
                lineDtos.Add(lineDto);

                linRev.StartStation = lineDto.EndStation;
                linRev.EndStation = lineDto.StartStation;
                lineDtos.Add(linRev);

            }
            _response.Data = lineDtos;
            return _response;
        }

        public async Task<BaseResponse> AddAdsAsync(string customerId,PassangerAddAdsDto adsRequest)
        {
            var availableToAdd = await _context.Advertisments
                        .AnyAsync(x => x.CustomerId == customerId
                        && x.organizationId == adsRequest.organizationId
                        && x.Name == adsRequest.Name);

            if (!availableToAdd)
            {
                _response.Status = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = _localization["AdsExist"].Value;
                return _response;
            }

            var newAds = _mapper.Map<Advertisment>(adsRequest);
            newAds.Status = AdsStatus.UnderConfirm;
            newAds.ImageUrl = await _mediaSerivce.AddAsync(adsRequest.Image);
            newAds.CustomerId = customerId;

            await _context.AddAsync(newAds);
            _ = await _context.SaveChangesAsync();

            _response.Message = _localization["UnderConfirm"].Value;
            return _response;
        }
        public async Task<BaseResponse>LinesVehiclesCountAsync(string orgId)
        {
            var trips = _context.Trips
                .Where(x => x.OrganizationId == orgId)
                .Select(x => new LinesVehiclesCountDto
                {
                    LineId = x.LineId,
                    Strat = x.Line.Start.Name,
                    End = x.Line.End.Name,
                    VehiclesCount = x.TimesTable.Select(x=>x.VehicleId).Distinct().Count()
                });

            _response.Data = await trips.ToListAsync();
            return _response;
        }
        public async Task<BaseResponse> AddPackagesAsync(PackagesRequestDto model)
        {
            if (model.isPublic == false && model.TripId == 0)
                throw new BadRequestException(_localization["EnterTripInfo"]);
            if (model.isPublic == true && model.DriverId == null)
                throw new BadRequestException(_localization["EnterDriverInfo"]);

            /* if (await _context.Packages.AnyAsync(p => p.Name == model.Name && p.SenderId == model.SenderId&& (p.DriverId))
             {
                 throw new BadRequestException(_localization["PackagesExist"]);
             }*/

            Package package = _mapper.Map<Package>(model);
            package.Status = (int)PackageStatus.UnderConfirm;

            if (model.ImageFile is not null)
            {
                package.ImageUrl = await _mediaSerivce.AddAsync(model.ImageFile);
            }
            await _context.Packages.AddAsync(package);
            _ = await _context.SaveChangesAsync();
            //  _response.Data = package;
            _response.Message = _localization["AddPAckagesSuccess"];
            return _response;
        }
        public async Task<BaseResponse> UpdatePackagesAsync(PackagesRequestDto model, int packageId)
        {
            var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == packageId);

            if (package is null)
            {
                throw new KeynotFoundException(_localization["ObjectNotFound"].Value);
            }

            package.TripId = model.TripId;
            package.SenderId = model.SenderId;
            package.Price = model.Price;
            package.ReciverPhoneNumber = model.ReciverPhoneNumber;
            package.Description = model.Description;
            package.Name = model.Name;
            package.ReciverName = model.ReciverName;
            package.ReciverUserName = model.ReciverUserName;
            package.Weight = model.Weight;


            if (model.ImageFile is not null)
            {
                package.ImageUrl = await _mediaSerivce.UpdateAsync(package.ImageUrl, model.ImageFile);
            }

            var res = _context.Packages.Update(package);
            _ = await _context.SaveChangesAsync();
            _response.Data = res;
            _response.Message = _localization["PackageUpdateSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> RemovePackageAsync(int packageId)
        {
            var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == packageId);

            if (package is null)
            {
                throw new KeynotFoundException(_localization["ObjectNotFound"].Value);
            }
            _context.Packages.Remove(package);
            _ = await _context.SaveChangesAsync();
            _response.Message = _localization["RemovePackageSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> GetUserOrgPackagesAsync(string userName)
        {
            var packages = await _context.Packages.Where(p => p.SenderId == userName && p.TripId != 0).ToListAsync();
            var res = _mapper.Map<OrgPackagesDto>(packages);
            _response.Data = res;
            return _response;
        }
        public async Task<BaseResponse> GetUserPublicPackagesAsync(string userName)
        {
            var packages = await _context.Packages.Where(p => p.SenderId == userName && p.DriverId != null).ToListAsync();
            var res = _mapper.Map<PublicPackagesDto>(packages);
            _response.Data = res;
            return _response;
        }


    }
}
