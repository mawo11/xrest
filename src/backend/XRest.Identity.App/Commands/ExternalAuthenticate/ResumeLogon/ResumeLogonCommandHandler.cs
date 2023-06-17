using XRest.Identity.App.Services;
using XRest.Identity.Contracts.ExternalAuthenticate.Request;
using XRest.Identity.Contracts.ExternalAuthenticate.Responses;
using XRest.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XRest.Identity.App.Commands.ExternalAuthenticate.Login;

public sealed class ResumeLogonCommandHandler : IRequestHandler<ResumeLogonCommand, ExternalLoginResponse>
{
	private readonly ExternalLoginResponse Fail = new() { Success = false };

	private readonly IMemoryCache _memoryCache;
	private readonly ILogger<ResumeLogonCommandHandler> _logger;
	private readonly IAccountRepository _accountRepository;
	private readonly IConfiguration _configuration;
	private readonly IDataProtector _dataProtectionProvider;

	public ResumeLogonCommandHandler(IMemoryCache memoryCache,
		ILogger<ResumeLogonCommandHandler> logger,
		IAccountRepository accountRepository,
		IConfiguration configuration,
		IDataProtectionProvider dataProtectionProvider)
	{
		_dataProtectionProvider = dataProtectionProvider.CreateProtector("Halo_Prot");
		_memoryCache = memoryCache;
		_logger = logger;
		_accountRepository = accountRepository;
		_configuration = configuration;
	}

	public async Task<ExternalLoginResponse> Handle(ResumeLogonCommand request, CancellationToken cancellationToken)
	{
		try
		{
			if (string.IsNullOrEmpty(request.Data))
			{
				_logger.LogError("Brak tokenu");
				return Fail;
			}

			var decodeToken = _dataProtectionProvider.Unprotect(request.Data);
			var requestData = CustomJsonSerializer.Deserialize<ExternalAuthenticateLogonRequest>(decodeToken);

			if (requestData == null)
			{
				_logger.LogError("brak danych");
				return Fail;
			}

			if (request.CreateNewAccount)
			{
				return await CreateNewAccountAsync(requestData);
			}

			return await AddMethodToAccountAsync(requestData);

		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "ExternalLogonCommandHandler");
		}

		return Fail;
	}

	private async Task<ExternalLoginResponse> AddMethodToAccountAsync(ExternalAuthenticateLogonRequest request)
	{
		int userId = -1;
		Domain.ExternalProvider externalProvider = (Domain.ExternalProvider)request.ExternalProvider;

		var checkUser = await _accountRepository.GetAccountByEmailAsync(request.Email!);
		if (checkUser != null)
		{
			await _accountRepository.AddNewLoginMethodToAccountAsync(checkUser.Id, request.ExternalIdentifier!, externalProvider);
			var cacheKey = Guid.NewGuid().ToString();

			_memoryCache.Set($"extlog_{cacheKey}", userId, TimeSpan.FromSeconds(60));

			var url = _configuration.GetValue<string>($"ExternalAuthenticate:Client{(int)request.ClientType}url");

			return new ExternalLoginResponse()
			{
				Success = true,
				Url = string.Format(url, cacheKey)
			};
		}

		return Fail;
	}

	private async Task<ExternalLoginResponse> CreateNewAccountAsync(ExternalAuthenticateLogonRequest request)
	{
		Domain.ExternalProvider externalProvider = (Domain.ExternalProvider)request.ExternalProvider;

		var user = await _accountRepository.GetAccountByExternalIdAsync(externalProvider, request.ExternalIdentifier!);
		int userId;
		if (user == Domain.Account.Invalid)
		{
			userId = await _accountRepository.InsertNewAccountAsync(new Domain.NewAccountData
			{
				Email = request.Email,
				ExernalProvider = externalProvider,
				ExternalId = request.ExternalIdentifier,
				Firstname = request.Firstname,
				Lastname = request.Surname,
				Marketing = true,
				Terms = true,
				Password = Guid.NewGuid().ToString(),
			});
		}
		else
		{
			userId = user!.Id;
		}

		if (userId == -1 || userId == 0)
		{
			_logger.LogError("Nie udało sie utworzyć konta: {0}", CustomJsonSerializer.Serialize(request));
			return Fail;
		}

		var cacheKey = Guid.NewGuid().ToString();

		_memoryCache.Set($"extlog_{cacheKey}", userId, TimeSpan.FromSeconds(60));

		var url = _configuration.GetValue<string>($"ExternalAuthenticate:Client{(int)request.ClientType}url");

		return new ExternalLoginResponse()
		{
			Success = true,
			Url = string.Format(url, cacheKey)
		};
	}
}
