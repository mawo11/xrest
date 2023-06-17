using XRest.Identity.App.Domain;
using XRest.Identity.App.Resources;
using XRest.Identity.App.Services;
using XRest.Shared.Domain;
using XRest.Shared.Services;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace XRest.Identity.App.Notifications.NewAccountNotificationHandlers;

public class SendEmail : INotificationHandler<NewAccountNotification>
{
	private readonly IMailRepository _mailRepository;
	private readonly IConfiguration _configuration;
	private readonly IAccountOperationRepository _accountOperationRepository;
	private readonly IDateTimeProvider _dateTimeProvider;

	public SendEmail(IMailRepository mailRepository,
		IConfiguration configuration,
		IAccountOperationRepository accountOperationRepository,
		IDateTimeProvider dateTimeProvider)
	{
		_dateTimeProvider = dateTimeProvider;
		_mailRepository = mailRepository;
		_configuration = configuration;
		_accountOperationRepository = accountOperationRepository;
	}

	public async Task Handle(NewAccountNotification notification, CancellationToken cancellationToken)
	{
		var operation = new AccountOperation
		{
			AccountId = notification.AccountId,
			Created = _dateTimeProvider.Now,
			Token = Guid.NewGuid().ToString().Replace("-", string.Empty),
			OperationType = AccountOperationType.Activate
		};

		bool result = await _accountOperationRepository.AddAccountOperationAsync(operation);

		if (result)
		{
			await GenerateActivateMailAsync(operation.Token, notification.NewAccountData);
		}
	}

	private async Task GenerateActivateMailAsync(string token, NewAccountData newAccountData)
	{
		Mail mail = new()
		{
			Status = MailStatus.ToSend,
			Template = MailTemplate.AccountActivation,
			Address = newAccountData.Email!,
			Subject = Strings.ActiveAccount
		};

		string activationUrl = _configuration.GetValue<string>("IdentityWeb:ActivationUrl");
		mail.AddReplacement("firstname", newAccountData.Firstname! ?? newAccountData.Email!);
		mail.AddReplacement("url", activationUrl.Replace("{token}", token));

		await _mailRepository.SaveMailAsync(mail);
	}
}
