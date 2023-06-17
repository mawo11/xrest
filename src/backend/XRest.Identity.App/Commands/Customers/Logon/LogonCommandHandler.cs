using XRest.Identity.App.Domain;
using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Identity.App.Commands.Customers.Logon;

public sealed class LogonCommandHandler : IRequestHandler<LogonCommand, LogonData>
{
	private readonly static LogonData Error = new()
	{
		Status = LogonDataStatus.Error
	};

	private readonly IAccountRepository _accountRepository;
	private readonly ILogger<LogonCommandHandler> _logger;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IUserTokenGenerator _userTokenGenerator;

	public LogonCommandHandler(IAccountRepository accountRepository,
		ILogger<LogonCommandHandler> logger,
		IPasswordHasher passwordHasher,

		IUserTokenGenerator userTokenGenerator)
	{
		_userTokenGenerator = userTokenGenerator;
		_accountRepository = accountRepository;
		_logger = logger;
		_passwordHasher = passwordHasher;
	}

	public async Task<LogonData> Handle(LogonCommand request, CancellationToken cancellationToken)
	{
		if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
		{
			return Error;
		}

		_logger.LogInformation("SendResetPasswordCommandHandler: {Email}", request.Email);

		var user = await _accountRepository.GetAccountByEmailAsync(request.Email);
		if (user == null ||  user.Islocked || user.IsDeleted)
		{
			return Error;
		}

		var encodedPassword = _passwordHasher.HashPassword(request.Password);

		if (user.Password != encodedPassword)
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
}