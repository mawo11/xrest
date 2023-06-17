using XRest.Identity.App.Services;
using XRest.Identity.Contracts.ExternalAuthenticate.Responses;
using XRest.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XRest.Identity.App.Commands.ExternalAuthenticate.Login;

public sealed class ExternalLogonCommandHandler : IRequestHandler<ExternalLogonCommand, ExternalLoginResponse>
{
	private readonly ExternalLoginResponse Fail = new() { Success = false };

	private readonly IMemoryCache _memoryCache;
	private readonly ILogger<ExternalLogonCommandHandler> _logger;
	private readonly IAccountRepository _accountRepository;
	private readonly IConfiguration _configuration;
	private readonly IDataProtector _dataProtectionProvider;

	public ExternalLogonCommandHandler(IMemoryCache memoryCache,
		ILogger<ExternalLogonCommandHandler> logger,
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

	public async Task<ExternalLoginResponse> Handle(ExternalLogonCommand request, CancellationToken cancellationToken)
	{
		try
		{
			Domain.ExternalProvider externalProvider = (Domain.ExternalProvider)request.Request.ExternalProvider;

			var checkUser = await _accountRepository.GetAccountByEmailAsync(request.Request.Email!);
			if (checkUser != null)
			{
				var externalAuthenticateAppUrl = _configuration.GetValue<string>("ExternalAuthenticate:ExternalAuthenticateAppUrl");
				var serializedData = CustomJsonSerializer.Serialize(request.Request);
				var encodeData = _dataProtectionProvider.Protect(serializedData);
				var askUrl = CombineUrl(externalAuthenticateAppUrl, $"authenticate/ask?data={encodeData}&email={request.Request.Email}&provider={request.Request.ExternalProvider}");

				return new ExternalLoginResponse()
				{
					Success = true,
					Url = askUrl
				};
			}

			int userId = -1;

			var user = await _accountRepository.GetAccountByExternalIdAsync(externalProvider, request.Request.ExternalIdentifier!);
			if (user == Domain.Account.Invalid)
			{
				userId = await _accountRepository.InsertNewAccountAsync(new Domain.NewAccountData
				{
					Email = request.Request.Email,
					ExernalProvider = externalProvider,
					ExternalId = request.Request.ExternalIdentifier,
					Firstname = request.Request.Firstname,
					Lastname = request.Request.Surname,
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
				_logger.LogError("Nie udało sie utworzyć konta: {0}", CustomJsonSerializer.Serialize(request.Request));
				return Fail;
			}

			var cacheKey = Guid.NewGuid().ToString();

			_memoryCache.Set($"extlog_{cacheKey}", userId, TimeSpan.FromSeconds(60));

			var url = _configuration.GetValue<string>($"ExternalAuthenticate:Client{(int)request.Request.ClientType}url");

			return new ExternalLoginResponse()
			{
				Success = true,
				Url = string.Format(url, cacheKey)
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "ExternalLogonCommandHandler");
		}

		return Fail;
	}

	private static string CombineUrl(string uri1, string uri2)
	{
		return $"{uri1.TrimEnd('/')}/{uri2}";
	}

}
