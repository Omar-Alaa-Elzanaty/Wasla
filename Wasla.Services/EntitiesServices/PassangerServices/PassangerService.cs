using Microsoft.Extensions.Localization;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
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

        public PassangerService
            (WaslaDb context,
            IStringLocalizer<PassangerService> localization,
            IMediaSerivce mediaSerivce)
        {
            _context = context;
            _response = new();
            _localization = localization;
            _mediaSerivce = mediaSerivce;
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
    }
}
