using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Helpers.Statics;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Account> _userManager;
        public PassangerService
            (
            WaslaDb context,
            IStringLocalizer<PassangerService> localization,
            IMediaSerivce mediaSerivce,
            IMapper mapper,
            UserManager<Account> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _response = new();
            _localization = localization;
            _mediaSerivce = mediaSerivce;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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
        public async Task<BaseResponse> ReservationAsync(ReservationDto order, string customerId)
        {

            List<Reservation> completeReserve = new List<Reservation>();
            //TODO: need to make reservation to every name in reservation dto seatInfo

            if (customerId is null)
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
                        _context.Seats.Add(new Seat() { setNum = set.SeatNum, TripTimeTableId = order.TripId });
                        _context.SaveChanges();
                        completeReserve.Add(new Reservation()
                        {
                            SetNum = set.SeatNum,
                            PassengerName = set.Name,
                            IsRide = false,
                            OnRoad = order.OnRoad,
                            CustomerId = customerId,
                            ReservationDate = DateTime.Now,
                            TriptimeTableId = order.TripId,
                            LocationDescription = order.LocationDescription,
                            QrCodeUrl = "Qr Code file Uri",//await _mediaSerivce.AddAsync(set.QrCodeFile)
                            CompnayName = tripTimeTable!.Trip.Organization.Name,
                            StartTime = tripTimeTable.StartTime,
                            EndTime = tripTimeTable.ArriveTime
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

                    await _userManager.UpdateAsync(customer);
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
        public async Task<BaseResponse> OrganizationRateAsync(OrganizationRateDto model, string customerId)
        {
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
        public async Task<BaseResponse> OrganizationRateRemoveAsync(string organizationId, string customerId)
        {
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

        public async Task<BaseResponse> AddAdsAsync(PassangerAddAdsDto adsRequest, string customerId)
        {
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
            package.Status = PackageStatus.UnderConfirm;


            if (model.ImageFile is not null)
            {
                package.ImageUrl = await _mediaSerivce.AddAsync(model.ImageFile);
            }
            await _context.Packages.AddAsync(package);

            var sender = await _context.Customers.FindAsync(model.SenderId);

            if (model.isPublic)
            {
                var trip = await _context.PublicDriverTrips.Where(x => x.PublicDriverId == model.DriverId)
                    .OrderByDescending(x => x.StartDate).FirstAsync();

                await _context.Notifications.AddAsync(new Notification()
                {
                    AccountId = trip.PublicDriverId,
                    Title = _localization["DriverNewPackageRequestTopic"].Value,
                    Description = _localization["DriverNewPackageRequestDescrption"].Value.Replace("Name", $"{sender.FirstName + ' ' + sender.LastName}"),
                    Type = NotificationType.PackageRequest
                });
            }
            else
            {
                var trip = await _context.Trips.FindAsync(model.TripId);
                await _context.Notifications.AddAsync(new Notification()
                {
                    AccountId = trip.Organization.Id,
                    Title = _localization["DriverNewPackageRequestTopic"].Value,
                    Description = _localization["DriverNewPackageRequestDescrption"].Value.Replace("Name", $"{sender.FirstName + ' ' + sender.LastName}"),
                    Type = NotificationType.PackageRequest
                });
            }

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
        public async Task<BaseResponse> GetUserOrgPackagesAsync(string customerId)
        {

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }
            var packages = await _context.Packages.Where(p => p.SenderId == customerId && p.TripId != 0).ToListAsync();
            var res = _mapper.Map<OrgPackagesDto>(packages);
            _response.Data = res;
            return _response;
        }
        public async Task<BaseResponse> GetUserPublicPackagesAsync(string customerId)
        {
            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }
            var packages = await _context.Packages.Where(p => p.SenderId == customerId && p.DriverId != null).ToListAsync();
            var res = _mapper.Map<List<PublicPackagesDto>>(packages);
            _response.Data = res;
            return _response;
        }
        public async Task<BaseResponse> GetProfile(string customerId)
        {
            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }
            var user = await _context.Customers.FindAsync(customerId);

            if (user is null)
            {
                return BaseResponse.GetErrorException(HttpStatusErrorCode.NotFound, _localization["UserNameNotFound"].Value);
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
        public async Task<BaseResponse> EditProfile(string userId, EditCustomerProfileDto model)
        {
            var customer = await _context.Customers.FindAsync(userId);

            if (customer is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, "User not found.");
            }

            customer.FirstName = model.FullName.Split(' ')[0];
            customer.LastName = model.FullName.Split(' ')[1];
            customer.Birthdate = model.Birthdate;
            customer.Gender = model.Gender;
            customer.Email = model.Email;
            customer.PhoneNumber = model.PhoneNumber;
            customer.UserName = model.UserName;

            if (model.Photo is null && customer.PhotoUrl != null)
            {
                await _mediaSerivce.DeleteAsync(customer.PhotoUrl);
            }
            else if (model.Photo is not null)
            {
                customer.PhotoUrl = customer.PhotoUrl.IsNullOrEmpty() ?
                     await _mediaSerivce.AddAsync(model.Photo) : await _mediaSerivce.UpdateAsync(customer.PhotoUrl, model.Photo);
            }

            await _userManager.UpdateAsync(customer);

            return _response;
        }
        public async Task<BaseResponse> SearchByUserName(string userName)
        {
            if (userName is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }
            var user = await _context.Customers.FirstOrDefaultAsync(c => c.UserName == userName);

            if (user is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["UserNameNotFound"].Value);
            }

            var customer = _mapper.Map<SearchByUserNameDto>(user);

            _response.Data = customer;
            return _response;
        }
        public async Task<BaseResponse> GetInComingReservations(string customerId)
        {
            //var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result?.Id;

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }

            var user = await _context.Customers.FindAsync(customerId);

            if (user is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["UserNameNotFound"].Value);
            }

            return await GetReservationOnMatchDate(x => x.ReservationDate > DateTime.UtcNow&&x.TripTimeTable.StartTime>DateTime.UtcNow, user);
        }
        public async Task<BaseResponse> GetFirstInComingReservations(string customerId)
        {
            //var customerId = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).Result?.Id;

            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }

            var user = await _context.Customers.FindAsync(customerId);

            if (user is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["UserNameNotFound"].Value);
            }

            return await GetFirstReservationOnMatchDate(x => x.ReservationDate > DateTime.UtcNow && x.TripTimeTable.StartTime > DateTime.UtcNow, user);
        }
        public async Task<BaseResponse> GetEndedReservations(string customerId)
        {
            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }
            var user = await _context.Customers.FindAsync(customerId);

            if (user is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["UserNameNotFound"].Value);
            }

            return await GetReservationOnMatchDate(x => x.TripTimeTable!.ArriveTime <= DateTime.Now, user);
        }
        public async Task<BaseResponse> GetTripSuggestion(string customerId)
        {
            if (customerId is null)
            {
                throw new BadRequestException(_localization["Unauthorized"].Value);
            }

            var x = await _context.Reservations.Where(x => x.TriptimeTableId != null && x.TripTimeTable != null
                                                               && x.TripTimeTable.StartTime >= DateTime.Now && x.CustomerId == customerId)
                                               .ToListAsync();

            var lastTrips = x.DistinctBy(x => x.TriptimeTableId).Take(20).Select(x => x.TripTimeTable).ToList();

            if (lastTrips.Count < 5)
            {
                var first3AvailableTrips = await _context.TripTimeTables.Where(x => x.StartTime >= DateTime.Now)
                    .Select(x => new SuggestionTripsDto()
                    {
                        ComapnyName = x.Trip.Organization.Name,
                        CompanyRating = (x.Trip.Organization.Rates.Count != 0) ? x.Trip.Organization.Rates.Average(t => t.Rate) : 0,
                        From = x.IsStart == true ? x.Trip.Line.Start.Name : x.Trip.Line.End.Name,
                        To = x.IsStart == true ? x.Trip.Line.Start.Name : x.Trip.Line.End.Name,
                        ArrivalTime = x.ArriveTime,
                        StartTime = x.StartTime,
                        Id = x.Id,
                        price = (x.Trip != null) ? x.Trip.Price : 0,
                        ImageUrl = x.Trip.Organization.LogoUrl
                    }).Take(20).ToListAsync();

                _response.Data = first3AvailableTrips;
                return _response;
            }

            var suggestionsTrips = lastTrips.Select(x => new SuggestionTripsDto()
            {
                ComapnyName = x!.Trip.Organization.Name,
                CompanyRating = x.Trip.Organization.Rates.Average(t => t.Rate),
                From = x.IsStart == true ? x.Trip.Line.Start.Name : x.Trip.Line.End.Name,
                To = x.IsStart == true ? x.Trip.Line.Start.Name : x.Trip.Line.End.Name,
                ArrivalTime = x.ArriveTime,
                StartTime = x.StartTime,
                Id = x.Id,
                price = x.Trip.Price,
                ImageUrl = x.Trip.Organization.LogoUrl
            }).ToList();

            _response.Data = suggestionsTrips;

            return _response;
        }
        private async Task<BaseResponse> GetReservationOnMatchDate(Func<Reservation, bool> match, Customer customer)
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
        private async Task<BaseResponse> GetFirstReservationOnMatchDate(Func<Reservation, bool> match, Customer customer)
        {
            var inComingTrips = customer.Reservations.Where(match)
                .Where(x => x.TriptimeTableId != null).OrderBy(x => x.TripTimeTable.StartTime)
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
            }).FirstOrDefault();

            await Task.CompletedTask;

            _response.Data = inComingTrips;
            return _response;
        }

        public async Task<BaseResponse> CreateFollowRequestAsync(string senderId, FollowDto followDto)
        {
            var requestExist = _context.FollowRequests.Any(f => f.SenderId == senderId && f.FollowerId == followDto.FollowerId);
            if (requestExist)
                return BaseResponse.GetErrorException(HttpStatusErrorCode.BadRequest, "this request already exist");
            var req = new FollowRequests()
            {
                SenderId = senderId,
                FollowerId = followDto.FollowerId
            };

            var Passenger = await _context.Customers.FindAsync(senderId);

            var notification = new Notification()
            {
                AccountId = followDto.FollowerId,
                Title = _localization["FollowReqeustTopic"].Value,
                Description = _localization["FollowReqeustDescription"].Value.Replace("Name", $"{Passenger.FirstName + ' ' + Passenger.LastName}"),
                Type = NotificationType.FollowReqeust
            };

            await _context.AddAsync(notification);
            await _context.FollowRequests.AddAsync(req);
            await _context.SaveChangesAsync();
            _response.Message = _localization["createFollowRequestSuccess"].Value;
            return _response;
        }


        public async Task<BaseResponse> ConfirmFollowRequestAsync(string userId, string senderId)
        {
            var followRequest = await _context.FollowRequests.SingleOrDefaultAsync(f => f.SenderId == senderId && f.FollowerId == userId);
            if (followRequest is null)
                return BaseResponse.GetErrorException(HttpStatusErrorCode.NotFound, "this request not exist");
            var follow = new UserFollow()
            {
                CustomerId = senderId,
                FollowerId = userId
            };
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    await _context.UserFollows.AddAsync(follow);
                    _context.FollowRequests.Remove(followRequest);
                    await _context.SaveChangesAsync();
                    _response.Message = _localization["createFollowSuccess"].Value;
                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                }
            }
            return _response;
        }

        public async Task<BaseResponse> DeleteFollowRequestAsync(string userId, string senderId)
        {
            var request = await _context.FollowRequests.SingleOrDefaultAsync(f => f.SenderId == senderId && f.FollowerId == userId);
            if (request is null)
                return BaseResponse.GetErrorException(HttpStatusErrorCode.NotFound, _localization["FollowRequestExist"].Value);

            _context.FollowRequests.Remove(request);
            await _context.SaveChangesAsync();
            _response.Message = _localization["RemoveFollowRequestSuccess"].Value;
            return _response;
        }

        public async Task<BaseResponse> DeleteFollowerAsync(string userId, FollowDto followDto)
        {
            var follow = await _context.UserFollows.SingleOrDefaultAsync(f => f.CustomerId == followDto.FollowerId && f.FollowerId == userId);
            if (follow is null)
                return BaseResponse.GetErrorException(HttpStatusErrorCode.NotFound, _localization["FollowNotFound"].Value);
            _context.UserFollows.Remove(follow);
            await _context.SaveChangesAsync();
            _response.Message = _localization["RemoveFollowSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> SearchUser(string request)
        {
            var users = await _context.Customers.Where(c => (c.UserName.StartsWith(request)) || (c.FirstName.StartsWith(request))).Select(c => new UserSearchProfileDto
            {
                UserId = c.Id,
                Name = c.FirstName,
                UserImage = c.PhotoUrl,
                UserName = c.UserName
            }).ToListAsync();

            _response.Data = users;
            return _response;
        }
        public async Task<BaseResponse> GetUserBySearch(string userId)
        {
            var user = await _context.Customers.FindAsync(userId);
            var userDto = new UserSearchProfileDto { UserId = user.Id, Name = user.FirstName, UserImage = user.PhotoUrl, UserName = user.UserName };
            _response.Data = userDto;
            return _response;
        }

        //public async Task<BaseResponse> DeleteFollowersAsync(DeleteFromFollowersCommand command)
        //{
        //    var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);

        //    if (user is null)
        //    {
        //        return BaseResponse.GetErrorException(System.Net.HttpStatusCode.Unauthorized, _localization["Unauthorized"].Value);
        //    }

        //    var customer = await _context.Customers.FindAsync(user.Id);

        //    var follower = customer!.Follows.Single(x => x.CustomerId == command.followerId);

        //    _context.UserFollows.Remove(follower);
        //    await _context.SaveChangesAsync();

        //    _response.Message = _localization["SuccessProcess"].Value;
        //    return _response;
        //}
        public async Task<BaseResponse> AcceptFollowRequestAsync(AcceptFollowRequestCommand command)
        {
            var followRequest = await _context.FollowRequests
                .SingleAsync(x => x.SenderId == command.SenderId && x.FollowerId == command.FolowerId);

            if (followRequest is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.Unauthorized, _localization["InvalidRequest"].Value);
            }

            var follow = new UserFollow()
            {
                CustomerId = followRequest.SenderId,
                FollowerId = followRequest.FollowerId
            };

            await _context.AddAsync(follow);
            await _context.SaveChangesAsync();

            _response.Message = _localization["SuccessProcess"].Value;
            return _response;
        }
        public async Task<BaseResponse> DisplayFollowingRequestsAsync(string customerId)
        {
            if (customerId is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.Unauthorized, _localization["Unauthorized"].Value);
            }

            var requests = await _context.FollowRequests.Where(x => x.FollowerId == customerId)
                                        .Select(x => new DisplayFollowingRequestsDto()
                                        {
                                            FollowingId = x.SenderId,
                                            Name = x.Sender.FirstName + ' ' + x.Sender.LastName,
                                            PhotoUrl = x.Sender.PhotoUrl,
                                            UserName = x.Sender.UserName
                                        }).ToListAsync();
            _response.Data = requests;
            return _response;
        }

        public async Task<BaseResponse> GetFollowers(string userId)
        {
            var followers = await _context.UserFollows.Where(x => x.FollowerId == userId)
                        .Select(x => new GetFollowerQueryDto()
                        {
                            Id = x.CustomerId,
                            FullName = x.Customer.FirstName + ' ' + x.Customer.LastName,
                            PhotoUrl = x.Customer.PhotoUrl,
                            UserName = x.Customer.UserName
                        }).ToListAsync();


            _response.Data = followers;
            return _response;
        }

        public async Task<BaseResponse> GetFollowing(string userId)
        {
            var following = await _context.UserFollows.Where(x => x.CustomerId == userId)
                          .Select(x => new GetFollowerQueryDto()
                          {
                              Id = x.FollowerId,
                              FullName = x.Follower.FirstName + ' ' + x.Follower.LastName,
                              PhotoUrl = x.Follower.PhotoUrl,
                              UserName = x.Follower.UserName
                          }).ToListAsync();

            _response.Data = following;
            return _response;
        }

        public async Task<BaseResponse> GetTripsForUserAsync(string orgId, string lineName)
        {
            var trips = await _context.TripTimeTables.Where(t => t.Trip.OrganizationId == orgId &&
            (t.Trip.Line.Start.Name.StartsWith(lineName) ||
            t.Trip.Line.End.Name.StartsWith(lineName))).Select(t => new TripForUserDto
            {
                Price = t.Trip.Price,
                From = t.Trip.Line.Start.Name,
                To = t.Trip.Line.End.Name,
                orgName = t.Trip.Organization.Name,
                AvailablePackageSpace = t.AvailablePackageSpace,
                AvailableSets = t.RecervedSeats.Count()

            }).ToListAsync();
            //  var tripRes = _mapper.Map<List<TripForUserDto>>(trips);
            _response.Data = trips;
            return _response;
        }

        public async Task<BaseResponse> GetTripsForUserWithToAndFromAsync(string orgId, string from, string to)
        {
            var trips = await _context.TripTimeTables.Where(t => t.Trip.OrganizationId == orgId && (t.Trip.Line.Start.Name == from || t.Trip.Line.End.Name == to)).ToListAsync();
            var tripRes = _mapper.Map<List<TripForUserDto>>(trips);
            _response.Data = tripRes;
            return _response;
        }

        public async Task<BaseResponse> FollowersLocation(string userId)
        {
            var user = await _context.Customers.FindAsync(userId);

            if (user is null)
            {
                _response.IsSuccess = false;
                _response.Status = System.Net.HttpStatusCode.NotFound;
                _response.Message = "user not found.";

                return _response;
            }

            var followers = user.Follows.Select(x => x.FollowerId)
                                .ToList();

            followers.Add(userId);

            var followersInTrips = await _context.Reservations
                            .Where(x => followers.Contains(x.CustomerId) && x.TripTimeTable != null &&
                            x.StartTime <= DateTime.Now && x.EndTime >= DateTime.Now)
                            .DistinctBy(x => x.CustomerId)
                            .Select(x => new FollowerCurrentTripDto()
                            {
                                Id = x.CustomerId,
                                FullName = x.Customer.FirstName + ' ' + x.Customer.LastName,
                                Langtitude = x.TripTimeTable.Langtitude,
                                Latitude = x.TripTimeTable.Latitude,
                                UserName = x.Customer.UserName,
                                CustomerImageUrl = x.Customer.PhotoUrl,
                                CompanyImageUrl = x.TripTimeTable.Trip.Organization.LogoUrl
                            }).ToListAsync();

            followersInTrips.AddRange(await _context.PublicDriverTripReservation
                                        .Where(x => followers.Contains(x.CustomerId))
                                        .Where(x => x.PublicDriverTrip.StartDate <= DateTime.Now && x.PublicDriverTrip.EndDate >= DateTime.Now)
                                        .Select(x => new FollowerCurrentTripDto()
                                        {
                                            CompanyImageUrl = x.PublicDriverTrip.PublicDriver.PhotoUrl,
                                            FullName = x.PublicDriverTrip.PublicDriver.FirstName + ' ' + x.PublicDriverTrip.PublicDriver.LastName,
                                            CustomerImageUrl = x.Customer.PhotoUrl,
                                            Id = x.CustomerId,
                                            Langtitude = x.PublicDriverTrip.Langtitude,
                                            Latitude = x.PublicDriverTrip.Latitude,
                                            UserName = x.Customer.UserName
                                        }).ToListAsync());

            followersInTrips = followersInTrips.DistinctBy(x => x.UserName).ToList();

            _response.Data = followersInTrips;
            return _response;
        }

        public async Task<BaseResponse> PackagesLocations(string userId)
        {
            var packages = await _context.Packages
                               .Where(x => x.SenderId == userId && x.Trip.StartTime <= DateTime.Now && x.Trip.ArriveTime >= DateTime.Now)
                               .Select(x => new PackageLocationDto()
                               {
                                   Id = x.Id,
                                   ImageUrl = x.ImageUrl,
                                   Name = x.Name,
                                   ReciverName = x.ReciverName,
                                   ReciverUserName = x.ReciverUserName,
                                   Langtitude = x.Trip.Langtitude,
                                   Latitude = x.Trip.Latitude
                               }).ToListAsync();

            _response.Data = packages;
            return _response;
        }



        public async Task<BaseResponse> SearchTripsForUserAsync(string from, string to, DateTime? date)
        {
            var today = DateTime.Today;
            List<SearchOrgTripsForUser> orgTrips = await _context.TripTimeTables
                 .Where(tt =>
                     ((tt.Trip.Line.Start.Name.Trim().ToLower().StartsWith(from.Trim().ToLower()) && tt.Trip.Line.End.Name.Trim().ToLower().StartsWith(to.Trim().ToLower()) && tt.IsStart) ||
                      (tt.Trip.Line.Start.Name.Trim().ToLower().StartsWith(to.Trim().ToLower()) && tt.Trip.Line.End.Name.Trim().ToLower().StartsWith(from.Trim().ToLower()) && !tt.IsStart))
                      && (date.HasValue ? tt.StartTime.Date == date.Value.Date : tt.StartTime.Date >= today)

                     ).OrderBy(t => t.StartTime)
                 .Select(t => new SearchOrgTripsForUser
                 {
                     TripId = t.Id,
                     TripDate = t.StartTime.ToString("MM/dd/yyyy"),
                     TripDay = t.StartTime.ToString("dddd"),
                     TripStartTime = t.StartTime.ToString("h:mm tt"),
                     StartStation = t.Trip.Line.Start.Name,
                     EndStation = t.Trip.Line.End.Name,
                     ImgUrl = t.Trip.Organization.LogoUrl,
                     Price = t.Trip.Price,
                     OrgName = t.Trip.Organization.Name,
                     Rates = t.Trip.Organization.Rates

                 }).ToListAsync();
            List<SearchTripsForUser> publicTrips = await _context.PublicDriverTrips
               .Where(tt =>
                   ((tt.StartStation.Name.Trim().ToLower().StartsWith(from.Trim().ToLower()) && tt.EndStation.Name.Trim().ToLower().StartsWith(to.Trim().ToLower()) && tt.IsStart) ||
                    (tt.StartStation.Name.Trim().ToLower().StartsWith(to.Trim().ToLower()) && tt.EndStation.Name.Trim().ToLower().StartsWith(from.Trim().ToLower()) && !tt.IsStart))
                   && (date.HasValue ? tt.StartDate.Date == date.Value.Date : tt.StartDate.Date >= today) &&
                   tt.IsActive == true

                   ).OrderBy(t => t.StartDate)
               .Select(t => new SearchTripsForUser
               {
                   TripId = t.Id,
                   TripDate = t.StartDate.ToString("MM/dd/yyyy"),
                   TripDay = t.StartDate.ToString("dddd"),
                   TripStartTime = t.StartDate.ToString("h:mm tt"),
                   StartStation = t.StartStation.Name,
                   EndStation = t.EndStation.Name,
                   ImgUrl = t.PublicDriver.PhotoUrl,

               }).ToListAsync();

            _response.Data = new { orgTrips, publicTrips };
            return _response;
        }
        public async Task<BaseResponse> RequestPublicTrip(PassengerPublicTripRequestDto model, string userId)
        {
            var request = _mapper.Map<PublicDriverTripRequest>(model);
            request.CustomerId = userId;

            await _context.AddAsync(request);
            await _context.SaveChangesAsync();

            return _response;
        }
    }
}
