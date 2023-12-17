using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.Exceptions;

namespace Wasla.Services.PassangerServices
{
	public class PassangerService:IPassangerService
	{
		private readonly WaslaDb _context;
		private readonly BaseResponse _response;
		private readonly IStringLocalizer<PassangerService> _localization;

		public PassangerService
			(WaslaDb context,
			IStringLocalizer<PassangerService> localization)
		{
			_context = context;
			_response = new();
			_localization = localization;
		}

		public async Task<BaseResponse> SetsRecordsAsync(int tripId)
		{
			var trip = await _context.Trips.FindAsync(tripId);

			if (trip is null)
			{
				throw new KeyNotFoundException(_localization["ObjectNotFound"].Value);
			}

			var reciveredSets = trip.RecervedSets.Select(s => s.setNum).ToHashSet();

			var sets = new List<SetStatusDto>();

			for (int setNum = 1; setNum <= trip.Capacity; setNum++)
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
		public async Task<BaseResponse> ReservationAsync(List<int>SetsNum,int tripId,string custId)
		{
			//TODO: need more work
			List<int>failed = new List<int>();
			List<Reservation> completeReserve = new List<Reservation>();

			foreach(var set in SetsNum)
			{
				try
				{
					_context.Seats.Add(new Seat() { setNum = set, TripId = tripId });
					_context.SaveChanges();
					completeReserve.Add(new Reservation()
					{
						SetNum=set,
						CustomerId=custId,
						ReservationDate=DateTime.Now,
						TripId=tripId
					});
				}
				catch
				{
					failed.Add(set);
				}
			}
			using (var trans =await _context.Database.BeginTransactionAsync())
			{
				try
				{
					await _context.AddAsync(completeReserve);

					var customer = await _context.Customers.FindAsync(custId);
					var trip = await _context.Trips.FindAsync(tripId);

					customer.points += completeReserve.Count * trip.Points;

					await _context.SaveChangesAsync();
					await trans.CommitAsync();
				}
				catch
				{
					await trans.RollbackAsync();
				}
			}


			if (failed.IsNullOrEmpty())
			{
				_response.Data = failed; 
				return _response;
			}

			_response.Message = _localization["SuccessProcess"].Value;
			return _response;
		}
		public async Task<BaseResponse>OrganizationRateAsync(OrganizationRate model)
		{
			var customer = _context.Customers.SingleOrDefault(c => c.Id == model.CustomerId);
			var rate = customer?.OrganizationRates.FirstOrDefault(r => r.OrgId == model.OrgId);

			if (customer is not null && rate is null)
			{
				customer.OrganizationRates.Add(model);

				_response.Message = _localization["AddRate"].Value;
			}
			else if(customer is not null && rate is not null)
			{
				rate.OrgId = model.OrgId;
				rate.CustomerId = model.CustomerId;
				rate.Rate = model.Rate;

				_response.Message = _localization["UpdateRate"].Value;
			}

			await _context.SaveChangesAsync();

			return _response;
		}
		public async Task<BaseResponse>OrganizationRateRemoveAsync(string organizationId,string customerId)
		{
			var customer = _context.Customers.SingleOrDefault(c => c.Id == customerId);
			var rate = customer?.OrganizationRates
					.SingleOrDefault(r => r.OrgId == organizationId);

			if(customer is null || rate is null)
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
