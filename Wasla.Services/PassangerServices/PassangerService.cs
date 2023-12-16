using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
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
			await _context.AddAsync(completeReserve);
			await _context.SaveChangesAsync();

			if (failed.IsNullOrEmpty())
			{
				_response.Data = failed; 
				return _response;
			}

			_response.Message = _localization["SuccessProcess"].Value;
			return _response;
		}
	}
}
