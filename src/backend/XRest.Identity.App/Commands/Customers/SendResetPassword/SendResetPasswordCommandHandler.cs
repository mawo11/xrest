using XRest.Identity.App.Domain;
using XRest.Identity.App.Resources;
using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Customers.Responses;
using XRest.Shared.Domain;
using XRest.Shared.Extensions;
using XRest.Shared.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XRest.Identity.App.Commands.Customers.SendResetPassword;

public sealed class SendResetPasswordCommandHandler : IRequestHandler<SendResetPasswordCommand, ResetPasswordResponse>
{
	private readonly static ResetPasswordResponse Error = new(ResetPasswordStatus.Error);
	private readonly static ResetPasswordResponse Ok = new(ResetPasswordStatus.Ok);

	private readonly IMailRepository _mailRepository;
	private readonly IConfiguration _configuration;
	private readonly IAccountRepository _accountRepository;
	private readonly ILogger<SendResetPasswordCommandHandler> _logger;
	private readonly IAccountOperationRepository _accountOperationRepository;
	private readonly IDateTimeProvider _dateTimeProvider;

	public SendResetPasswordCommandHandler(IMailRepository mailRepository,
		IConfiguration configuration,
		IAccountRepository accountRepository,
		ILogger<SendResetPasswordCommandHandler> logger,
		IAccountOperationRepository accountOperationRepository,
		IDateTimeProvider dateTimeProvider)
	{
		_mailRepository = mailRepository;
		_configuration = configuration;
		_accountRepository = accountRepository;
		_logger = logger;
		_accountOperationRepository = accountOperationRepository;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<ResetPasswordResponse> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
	{
		if (string.IsNullOrEmpty(request.Email))
		{
			return Error;
		}

		_logger.LogInformation("SendResetPasswordCommandHandler: {data}", CustomJsonSerializer.Serialize(request));

		var user = await _accountRepository.GetAccountByEmailAsync(request.Email);
		if (user == null || user.Islocked || user.IsDeleted)
		{
			return Error;
		}

		var operation = new AccountOperation
		{
			AccountId = user.Id,
			Created = _dateTimeProvider.Now,
			Token = Guid.NewGuid().ToString().Replace("-", string.Empty),
			OperationType = AccountOperationType.ResetPassword
		};

		bool result = await _accountOperationRepository.AddAccountOperationAsync(operation);

		if (result)
		{
			await GenerateResetPasswordMailAsync(operation.Token, request.Email);
		}

		return Ok;
	}

	private async Task GenerateResetPasswordMailAsync(string token, string email)
	{
		Mail mail = new()
		{
			Status = MailStatus.ToSend,
			Template = MailTemplate.ResetPasswod,
			Address = email,
			Subject = Strings.ResetPassword
		};

		string activationUrl = _configuration.GetValue<string>("IdentityWeb:ResetPasswordUrl");
		mail.AddReplacement("url", activationUrl.Replace("{token}", token));

		await _mailRepository.SaveMailAsync(mail);
	}
}
