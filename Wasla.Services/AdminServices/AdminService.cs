using AutoMapper;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Transactions;
using Wasla.DataAccess;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Statics;
using Wasla.Model.Models;
using Wasla.Services.EmailServices;
using Wasla.Services.Exceptions;
using Wasla.Services.MediaSerivces;

namespace Wasla.Services.AdminServices
{
	public class AdminService:IAdminService
	{
		private readonly WaslaDb _context;
		private readonly BaseResponse _response;
		private readonly IMapper _mapper;
		private readonly UserManager<Account> _userManager;
		private readonly IStringLocalizer<AdminService> _localization;
		private readonly IMailServices _mailService;
		private readonly IMediaSerivce _mediaService;
		public AdminService(
			WaslaDb dbContext,
			IMapper mapper,
			UserManager<Account> userManager,
			IStringLocalizer<AdminService> stringLocalizer,
			IMailServices mailService,
			IMediaSerivce mediaService)
		{
			_response = new();
			_context = dbContext;
			_mapper = mapper;
			_userManager = userManager;
			_localization = stringLocalizer;
			_mailService = mailService;
			_mediaService = mediaService;
		}
		public async Task<BaseResponse> DisplayOrganiztionRequestsAsync()
		{
			var requests = await _context.OrganizationsRegisters.ToListAsync();

			if(requests is null ||requests.Count == 0)
			{
				throw new NotFoundException(_localization["InvalidRequest"].Value);
			}

			_response.Data = requests;
			return _response;
		}
		public async Task<BaseResponse>ConfirmOrgnaizationRequestAsync(int requestId)
		{
			var request =await _context.OrganizationsRegisters.SingleOrDefaultAsync(r => r.RequestId == requestId);

			if (request is null)
			{
				throw new KeyNotFoundException(_localization["InvalidRequest"].Value);
			}

			var organization = _mapper.Map<Organization>(request);

			organization.UserName = request.Email;

			using (var transaction=await _context.Database.BeginTransactionAsync())
			{
				try
				{
					var result = await _userManager.CreateAsync(organization, request.Password);
					if (!result.Succeeded)
					{
						var errors = string.Empty;
						foreach (var error in result.Errors)
							errors += $"{error.Description},";
						throw new BadRequestException(errors);
					}

					var roleResult = await _userManager.AddToRoleAsync(organization, Roles.Role_Organization);
					if (!roleResult.Succeeded)
					{
						throw new ServerErrorException(_localization["RegisterFaild"].Value);
					}

					await _mailService.SendEmailAsync(
							mailTo: request.Email,
							subject: "Wasla Email Annoucment",
							body: "Your email has been activated,now you can login using your username and password");

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
		public async Task<BaseResponse>CancelOrganizationRequestAsync(int requestId)
		{
			var request = await _context.OrganizationsRegisters.FirstOrDefaultAsync(i => i.RequestId == requestId);

			if(request is null)
			{
				throw new BadRequestException(_localization["InvalidRequest"].Value);
			}

			_context.OrganizationsRegisters.Remove(request);
			return _response;
		}
	}
}
