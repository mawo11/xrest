using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Identity.App.Commands.Customers.ChangeMyPassword;

public sealed class ChangeMyPasswordCommandHandler : IRequestHandler<ChangeMyPasswordCommand, ApiOperationResult>
{
	private readonly static ApiOperationResult Error = new(ApiOperationResultStatus.Error);
	private readonly static ApiOperationResult Ok = new(ApiOperationResultStatus.Ok);

	private readonly IAccountRepository _accountRepository;
	private readonly IPasswordHasher _passwordHasher;
	private readonly ILogger<ChangeMyPasswordCommandHandler> _logger;

	public ChangeMyPasswordCommandHandler(IAccountRepository accountRepository,
		IPasswordHasher passwordHasher,
		ILogger<ChangeMyPasswordCommandHandler> logger)
	{
		_accountRepository = accountRepository;
		_passwordHasher = passwordHasher;
		_logger = logger;
	}

	public async Task<ApiOperationResult> Handle(ChangeMyPasswordCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("ResetPasswordCommandHandler: {AccountId}", request.AccountId);

		var user = await _accountRepository.GetAccountByIdAsync(request.AccountId);
		if (string.IsNullOrEmpty(request.Password) || user == null || user.Islocked || user.IsDeleted)
		{
			return Error;
		}

		string newPassword = _passwordHasher.HashPassword(request.Password);

		await _accountRepository.UpdateAccountPasswordAsync(request.AccountId, newPassword);

		return Ok;
	}
}
