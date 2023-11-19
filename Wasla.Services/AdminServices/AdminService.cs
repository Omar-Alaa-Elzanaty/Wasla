using AutoMapper;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Math.Field;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.EmailServices;
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
		private readonly IMailServices _mailService;
		public AdminService(
			WaslaDb dbContext,
			IMapper mapper,
			UserManager<Account> userManager,
			IStringLocalizer<AdminService> stringLocalizer,
			IMailServices mailService)
		{
			_response = new();
			_dbContext = dbContext;
			_mapper = mapper;
			_userManager = userManager;
			_localization = stringLocalizer;
			_mailService = mailService;
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
			await _mailService.SendEmailAsync(
					mailTo: request.Email,
					subject: "Wasla Email Annoucment",
					body: "Your email has been activated,now you can login using username and password");

			return _response;
		}
		public async Task<BaseResponse>CancelOrganizationRequestAsync(int requestId)
		{
			var request = await _dbContext.OrganizationsRegisters.FirstOrDefaultAsync(i => i.Id == requestId);

			if(request is null)
			{
				throw new BadRequestException(_localization["InvalidRequest"].Value);
			}

			_dbContext.OrganizationsRegisters.Remove(request);
			return _response;
		}
	}
}
