using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using XRest.Shared.Extensions;

namespace XRest.Identity.App.Commands.Customers.DisableAccount;

public sealed class DisableAccountCommandHandler : IRequestHandler<DisableAccountCommand, ApiOperationResult>
{
	private readonly static ApiOperationResult Error = new(ApiOperationResultStatus.Error);
	private readonly static ApiOperationResult Ok = new(ApiOperationResultStatus.Ok);

	private readonly IAccountRepository _accountRepository;
	private readonly ILogger<DisableAccountCommandHandler> _logger;

	public DisableAccountCommandHandler(IAccountRepository accountRepository, ILogger<DisableAccountCommandHandler> logger)
	{
		_accountRepository = accountRepository;
		_logger = logger;
	}

	public async Task<ApiOperationResult> Handle(DisableAccountCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("DisableAccountCommandHandler: {request}", CustomJsonSerializer.Serialize(request));
		try
		{
			if (request.AccountId > 0)
			{
				await _accountRepository.DisableAccountAsync(request.AccountId);
				return Ok;
			}

		}
		catch (Exception e)
		{
			_logger.LogCritical(e, "DisableAccountCommandHandler");
		}

		return Error;
	}
}
