using XRest.Identity.App.Commands.Customers.ResetPasswword;
using XRest.Identity.App.Domain;
using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using XRest.Shared.Services;

namespace XRest.Identity.App.Commands.Customers.ActivateAccount;

public sealed class ActivateAccountCommandHandler : IRequestHandler<ActivateAccountCommand, ActivateAccountResponse>
{
	private readonly static ActivateAccountResponse Error = new(ActivateAccountResponseStatus.Error);
	private readonly static ActivateAccountResponse Ok = new(ActivateAccountResponseStatus.Ok);

	private readonly IAccountRepository _accountRepository;
	private readonly IAccountOperationRepository _accountOperationRepository;
	private readonly IConfiguration _configuration;
	private readonly ILogger<ResetPasswordCommandHandler> _logger;
	private readonly IDateTimeProvider _dateTimeProvider;

	public ActivateAccountCommandHandler(IAccountRepository accountRepository,
		IAccountOperationRepository accountOperationRepository,
		IConfiguration configuration,
		ILogger<ResetPasswordCommandHandler> logger,
		IDateTimeProvider dateTimeProvider)
	{
		_accountRepository = accountRepository;
		_accountOperationRepository = accountOperationRepository;
		_configuration = configuration;
		_logger = logger;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<ActivateAccountResponse> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var operation = await _accountOperationRepository.GetAccountOperationByTokenAsync(request.Token);
			if (operation == AccountOperation.Invalid || operation.OperationType != AccountOperationType.Activate)
			{
				return Error;
			}

			int tokenValidMinutes = _configuration.GetValue<int>("Identity:CreateAccountTokenValidMinutes");

			if ((_dateTimeProvider.Now - operation.Created).TotalMinutes > tokenValidMinutes)
			{
				return Error;
			}

			await _accountRepository.SetAccountStatusAsync(operation.AccountId, AccountStatus.Active);
			await _accountOperationRepository.RemoveAccountOperationByTokenAsync(request.Token);

			return Ok;

		}
		catch (Exception e)
		{
			_logger.LogError(e, "ActivateAccountCommandHandler");
		}

		return Error;
	}
}