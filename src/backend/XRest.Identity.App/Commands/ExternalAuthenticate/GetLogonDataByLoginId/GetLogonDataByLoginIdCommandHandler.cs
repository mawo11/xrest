using XRest.Identity.App.Domain;
using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace XRest.Identity.App.Commands.ExternalAuthenticate.GetLogonDataByLoginId;

public sealed class GetLogonDataByLoginIdCommandHandler : IRequestHandler<GetLogonDataByLoginIdCommand, LogonData>
{
	private readonly static LogonData Error = new()
	{
		Status = LogonDataStatus.Error
	};

	private readonly IMemoryCache _memoryCache;
	private readonly IAccountRepository _accountRepository;
	private readonly IUserTokenGenerator _userTokenGenerator;

	public GetLogonDataByLoginIdCommandHandler(IMemoryCache memoryCache,
		IAccountRepository accountRepository,
		IUserTokenGenerator userTokenGenerator)
	{
		_memoryCache = memoryCache;
		_accountRepository = accountRepository;
		_userTokenGenerator = userTokenGenerator;
	}

	public async Task<LogonData> Handle(GetLogonDataByLoginIdCommand request, CancellationToken cancellationToken)
	{
		if (string.IsNullOrEmpty(request.LogonId))
		{
			return Error;
		}

		var cacheKey = $"extlog_{request.LogonId}";

		if (_memoryCache.TryGetValue(cacheKey, out int userId))
		{
			_memoryCache.Remove(cacheKey);

			var user = await _accountRepository.GetAccountByIdAsync(userId);
			if (user == null || user.Islocked || user.IsDeleted)
			{
				return Error;
			}

			return new LogonData
			{
				Email = user.Email,
				Firstname = user.Firstname,
				Lastname = user.Lastname,
				Status = LogonDataStatus.Ok,
				Phone = user.Phone,
				MustChangePassword = user.MustChangePassword,
				Token = await _userTokenGenerator.GenerateAsync(user.Id)
			};
		}

		return Error;
	}
}
