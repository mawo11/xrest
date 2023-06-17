using XRest.Identity.App.Domain;
using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using XRest.Shared.Services;

namespace XRest.Identity.App.Commands.Customers.ResetPasswword;

public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
{
	private readonly static ResetPasswordResponse Error = new(ResetPasswordStatus.Error);
	private readonly static ResetPasswordResponse Ok = new(ResetPasswordStatus.Ok);

	private readonly IAccountRepository _accountRepository;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IConfiguration _configuration;
	private readonly ILogger<ResetPasswordCommandHandler> _logger;
	private readonly IAccountOperationRepository _accountOperationRepository;
	private readonly IDateTimeProvider _dateTimeProvider;

	public ResetPasswordCommandHandler(IAccountRepository accountRepository,
		IPasswordHasher passwordHasher,
		IConfiguration configuration,
		ILogger<ResetPasswordCommandHandler> logger,
		IAccountOperationRepository accountOperationRepository,
		IDateTimeProvider dateTimeProvider)
	{
		_accountRepository = accountRepository;
		_passwordHasher = passwordHasher;
		_configuration = configuration;
		_logger = logger;
		_accountOperationRepository = accountOperationRepository;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var operation = await _accountOperationRepository.GetAccountOperationByTokenAsync(request.Token);
			if (operation == AccountOperation.Invalid || operation.OperationType != AccountOperationType.ResetPassword)
			{
				return Error;
			}

			int tokenValidMinutes = _configuration.GetValue<int>("Identity:ResetPasswordTokenValidMinutes");

			if ((_dateTimeProvider.Now - operation.Created).TotalMinutes > tokenValidMinutes)
			{
				return Error;
			}

			var user = await _accountRepository.GetAccountByIdAsync(operation.AccountId);
			if (string.IsNullOrEmpty(request.Password) || user == null || user.Islocked || user.IsDeleted)
			{
				return Error;
			}

			string newPassword = _passwordHasher.HashPassword(request.Password);

			await _accountRepository.UpdateAccountPasswordAsync(operation.AccountId, newPassword);
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
