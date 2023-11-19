using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
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
		private readonly IStringLocalizer<AdminService> _localization;
		public AdminService(
			WaslaDb dbContext,
			IMapper mapper,
			UserManager<Account> userManager,
			IStringLocalizer<AdminService> stringLocalizer)
		{
			_response = new();
			_dbContext = dbContext;
			_mapper = mapper;
			_userManager = userManager;
			_localization = stringLocalizer;
		}
		public async Task<BaseResponse> DisplayOrganiztionRequestsAsync()
		{
			var requests = await _dbContext.OrganizationsRegisters.ToListAsync();

			if(requests is null ||requests.Count == 0)
			{
				throw new NotFoundException(_localization["InvalidRequest"].Value);
			}

			_response.Data = requests;
			return _response;
		}
		public async Task<BaseResponse>ConfirmOrgnaizationRequestAsync(int requestId)
		{
			var request =await _dbContext.OrganizationsRegisters.SingleOrDefaultAsync(r => r.Id == requestId);

			if (request is null)
			{
				throw new KeyNotFoundException(_localization["InvalidRequest"].Value);
			}

			var organization = _mapper.Map<Organization>(request);

			using (var transaction=await _dbContext.Database.BeginTransactionAsync())
			{
				var result=await _userManager.CreateAsync(organization, request.Password);
				if (!result.Succeeded)
				{
					var errors = string.Empty;
					foreach (var error in result.Errors)
						errors += $"{error.Description},";
					throw new BadRequestException(errors);
				}
				result = await _userManager.AddToRoleAsync(organization, Roles.Role_Organization);
				if (!result.Succeeded)
				{
					var errors = string.Empty;
					foreach (var error in result.Errors)
						errors += $"{error.Description},";
					await transaction.RollbackAsync();
					throw new BadRequestException(errors);
				}
				await transaction.CommitAsync();
			}

			_response.Message = $"{request.Name} account confirmed";

			return _response;
		}
	}
}
