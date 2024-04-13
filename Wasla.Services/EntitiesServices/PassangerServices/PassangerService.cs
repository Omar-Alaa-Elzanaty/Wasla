using System.Linq;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Cms;
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
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Account>_userManager;
        public PassangerService
            (
            WaslaDb context,
            IStringLocalizer<PassangerService> localization,
            IMediaSerivce mediaSerivce,
            IMapper mapper,
            HttpContextAccessor httpContextAccessor,
            UserManager<Account> userManager)
        {
            _context = context;
            _response = new();
            _localization = localization;
            _mediaSerivce = mediaSerivce;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<BaseResponse> SeatsRecordsAsync(int tripId)
        {
            var trip = await _context.TripTimeTables.FindAsync(tripId);

            if (trip is null)
            {
                throw new KeyNotFoundException(_localization["ObjectNotFound"].Value);
            }

            var reciveredSets = trip.RecervedSeats.Select(s => s.setNum).ToHashSet();

            var sets = new List<SetStatusDto>();

            for (int setNum = 1; setNum <= trip.Vehicle.Capcity; setNum++)
            {
                sets.Add(new()
                {
                    SetNum = setNum,
                    ISAvailable = !reciveredSets.Contains(setNum)
                });
            }

            _response.Data = sets;

            return _response;

        }
        public async Task<BaseResponse> ReservationAsync(ReservationDto order)
        {

            List<Reservation> completeReserve = new List<Reservation>();
            //TODO: need to make reservation to every name in reservation dto seatInfo
            var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result.Id;

            if(customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }

            var tripTimeTable = await _context.TripTimeTables.FindAsync(order.TripId);
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var set in order.seats)
                    {
                        _context.Seats.Add(new Seat() { setNum = set.SeatNum, TripTmeTableId = order.TripId });
                        _context.SaveChanges();
                        completeReserve.Add(new Reservation()
                        {
                            SetNum = set.SeatNum,
                            CustomerId = customerId,
                            ReservationDate = DateTime.Now,
                            TriptimeTableId = order.TripId,
                            QrCodeUrl = "Qr Code file Uri",//await _mediaSerivce.AddAsync(set.QrCodeFile)
                            CompnayName=tripTimeTable!.Trip.Organization.Name,
                            StartTime=tripTimeTable.StartTime,
                            EndTime=tripTimeTable.ArriveTime
                        });

                    }
                    await _context.Reservations.AddRangeAsync(completeReserve);

                    var customer = await _context.Customers.FindAsync(customerId);
                    var trip = await _context.TripTimeTables.FindAsync(order.TripId);

                    if (customer is null || trip is null)
                    {
                        throw new KeynotFoundException(_localization["ReservationFail"].Value);
                    }
                    customer.points += completeReserve.Count * trip.Trip.Points;

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
                t.TriptimeTableId,
                t.QrCodeUrl,
                t.ReservationDate
            });

            return _response;
        }
        public async Task<BaseResponse> PassengerCancelReversionAsyn(int reverseId)
        {
            var reverse = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == reverseId);
            if (reverse == null)
                throw new NotFoundException("ReversenotFound");
            var seat = await _context.Seats.FirstOrDefaultAsync(s => s.setNum == reverse.SetNum);
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                _context.Seats.Remove(seat);
                _context.Reservations.Remove(reverse);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();

            }

            _response.Message = _localization["Cancel"].Value;
            return _response;

        }
        public async Task<BaseResponse> OrganizationRateAsync(OrganizationRateDto model)
        {
            var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result.Id;

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }

            var customer = _context.Customers.SingleOrDefault(c => c.Id == customerId);
            var rate = customer?.OrganizationRates.FirstOrDefault(r => r.OrgId == model.OrgId);

            if (customer is not null && rate is null)
            {
                rate = _mapper.Map<OrganizationRate>(model);
                rate.CustomerId = customerId;

                customer.OrganizationRates.Add(rate);

                _response.Message = _localization["AddRate"].Value;
            }
            else if (customer is not null && rate is not null)
            {
                rate.OrgId = model.OrgId;
                rate.CustomerId = customerId;
                rate.Rate = model.Rate;

                _response.Message = _localization["UpdateRate"].Value;
            }

            await _context.SaveChangesAsync();

            return _response;
        }
        public async Task<BaseResponse> OrganizationRateRemoveAsync(string organizationId)
        {
            var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result.Id;

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }

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

        public async Task<BaseResponse> AddAdsAsync(PassangerAddAdsDto adsRequest)
        {
            var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result.Id;

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }

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
        public async Task<BaseResponse> LinesVehiclesCountAsync(string orgId)
        {
            var trips = _context.Trips
                .Where(x => x.OrganizationId == orgId)
                .Select(x => new LinesVehiclesCountDto
                {
                    LineId = x.LineId,
                    Strat = x.Line.Start.Name,
                    End = x.Line.End.Name,
                    VehiclesCount = x.TimesTable.Select(x => x.VehicleId).Distinct().Count()
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
        public async Task<BaseResponse> GetUserOrgPackagesAsync()
        {
            var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result.Id;

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }
            var packages = await _context.Packages.Where(p => p.SenderId == customerId && p.TripId != 0).ToListAsync();
            var res = _mapper.Map<OrgPackagesDto>(packages);
            _response.Data = res;
            return _response;
        }
        public async Task<BaseResponse> GetUserPublicPackagesAsync()
        {
            var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result.Id;

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }
            var packages = await _context.Packages.Where(p => p.SenderId == customerId && p.DriverId != null).ToListAsync();
            var res = _mapper.Map<PublicPackagesDto>(packages);
            _response.Data = res;
            return _response;
        }
        public async Task<BaseResponse> GetProfile()
        {
            var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result.Id;

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }
            var user = await _context.Customers.FindAsync(customerId);

            if (user is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["UserNameNotFound"].Value);
            }

            var customer = _mapper.Map<DisplayCustomerProfileDto>(user);

            customer.Followers.TryAdd(_context.UserFollows.Where(x => x.CustomerId == user.Id)
                .Select(x => new Follow()
                {
                    Id = x.FollowerId,
                    Name = x.Follower.FirstName + ' ' + x.Follower.LastName
                }));

            customer.Following.TryAdd(
                _context.UserFollows.Where(x => x.FollowerId == user.Id)
                .Select(x => new Follow()
                {
                    Id = x.CustomerId,
                    Name = x.Customer.FirstName + " " + x.Customer.LastName
                }));

            _response.Data = customer;
            return _response;
        }
        public async Task<BaseResponse> GetInComingReservations()
        {
            var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result.Id;

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }

            var user = await _context.Customers.FindAsync(customerId);

            if (user is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["UserNameNotFound"].Value);
            }

            return await GetReservationOnMatchDate(x => x.ReservationDate > DateTime.Now, user);
        }
        public async Task<BaseResponse> GetEndedReservations()
        {
            var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result.Id;

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }
            var user = await _context.Customers.FindAsync(customerId);

            if (user is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["UserNameNotFound"].Value);
            }

            return await GetReservationOnMatchDate(x => x.ReservationDate <= DateTime.Now, user);
        }
        public async Task<BaseResponse>GetTripSuggestion()
        {
            var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result.Id;

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }

            var last3Trips = await _context.Reservations.Where(x => x.TriptimeTableId != null&&x.TripTimeTable != null 
                                                               && x.TripTimeTable.StartTime >= DateTime.Now&&x.CustomerId==customerId)
                                                   .DistinctBy(x=>x.TriptimeTableId).Select(x=>x.TripTimeTable).Take(3).ToListAsync();
            if (last3Trips.Count<3)
            {
                var first3AvailableTrips =await _context.TripTimeTables.Where(x => x.StartTime >= DateTime.Now)
                    .Take(3)
                    .Select(x => new SuggestionTripsDto()
                    {
                        ComapnyName = x.Trip.Organization.Name,
                        CompanyRating = x.Trip.Organization.Rates.Average(t => t.Rate),
                        From = x.IsStart == true ? x.Trip.Line.Start.Name : x.Trip.Line.End.Name,
                        To = x.IsStart == true ? x.Trip.Line.Start.Name : x.Trip.Line.End.Name,
                        ArrivalTime = x.ArriveTime,
                        StartTime = x.StartTime,
                        Id = x.Id,
                        price = x.Trip.Price
                    }).ToListAsync();

                _response.Data = first3AvailableTrips;
                return _response;
            }

            var suggestionsTrips = last3Trips.Select(x => new SuggestionTripsDto()
            {
                ComapnyName = x!.Trip.Organization.Name,
                CompanyRating = x.Trip.Organization.Rates.Average(t => t.Rate),
                From = x.IsStart == true ? x.Trip.Line.Start.Name : x.Trip.Line.End.Name,
                To = x.IsStart == true ? x.Trip.Line.Start.Name : x.Trip.Line.End.Name,
                ArrivalTime = x.ArriveTime,
                StartTime = x.StartTime,
                Id = x.Id,
                price = x.Trip.Price
            }).ToList();

            _response.Data = suggestionsTrips;

            return _response;
        }
        private async Task<BaseResponse> GetReservationOnMatchDate(Func<Reservation,bool> match,Customer customer)
        {
            var inComingTrips = customer.Reservations.Where(match)
                .Where(x => x.TriptimeTableId != null)
            .Select(x => new CustomerTicket()
            {
                StartStation = x.TripTimeTable.Trip.Line.Start.Name,
                EndStation = x.TripTimeTable.Trip.Line.End.Name,
                EndTime = x.TripTimeTable.ArriveTime,
                StartTime = x.TripTimeTable.StartTime,
                PassengerName = x.PassengerName,
                Price = x.TripTimeTable.Trip.Price,
                SeatNumber = x.SetNum,
                TripTimeTableId = (int)x.TriptimeTableId!
            }).ToList();

            await Task.CompletedTask;

            _response.Data = inComingTrips;
            return _response;
        }
    }
}
