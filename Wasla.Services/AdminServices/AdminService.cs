using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.Exceptions;

namespace Wasla.Services.AdminServices
{
	public class AdminService:IAdminService
	{
		private readonly WaslaDb _dbContext;
		private readonly BaseResponse _response;
		private readonly IMapper _mapper;
		private readonly UserManager<Account> _userManager;
		public AdminService(
			WaslaDb dbContext,
			IMapper mapper,
			UserManager<Account>userManager)
		{
			_response = new();
			_dbContext = dbContext;
			_mapper = mapper;
			_userManager = userManager;
		}
		public async Task<BaseResponse> DisplayOrganiztionRequestsAsync()
		{
			var requests = await _dbContext.OrganizationsRegisters.ToListAsync();

			if(requests is null ||requests.Count == 0)
			{
				throw new NotFoundException("NO requests");
			}

			_response.Data = requests;
			return _response;
		}
		public async Task<BaseResponse>ConfirmOrgnaizationRequestAsync(int requestId)
		{
			var request =await _dbContext.OrganizationsRegisters.SingleOrDefaultAsync(r => r.Id == requestId);

			if (request is null)
			{
				throw new KeyNotFoundException("NO such request found");
			}

			var account = new Account()
			{
				UserName = request.Email,
				Email = request.Email,
				PhoneNumber = request.PhoneNumber
			};
			var result = await _userManager.CreateAsync(account, request.Password);

			if (!result.Succeeded)
			{
				var errorList = string.Empty;
				foreach (var error in result.Errors)
				{
					errorList += error + " ,";
				}

				throw new ServerErrorException($"coudn't create account: {errorList}");
			}
			var organization = new Organization()
			{
				Name = request.Name,
				Address = request.Address,
				LogoUrl = request.ImageUrl,
				AccountId = account.Id,
				WebsiteLink = request.WebSiteLink
			};
			await _dbContext.AddAsync(organization);
			_= await _dbContext.SaveChangesAsync();

			_response.Message = $"{request.Name} account confirmed";
			_response.Data = new { 
				organization.Id,
				organization.Name,
				organization.Address,
				organization.LogoUrl
			};
			return _response;
		}
	}
}
