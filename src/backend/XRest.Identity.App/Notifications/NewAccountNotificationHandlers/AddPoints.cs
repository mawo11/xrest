using XRest.Identity.App.Domain;
using XRest.Identity.App.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using XRest.Shared.Services;

namespace XRest.Identity.App.Notifications.NewAccountNotificationHandlers;

public class AddPoints : INotificationHandler<NewAccountNotification>
{
	private readonly IAccountRepository _accountRepository;
	private readonly IConfiguration _configuration;
	private readonly IDateTimeProvider _dateTimeProvider;

	public AddPoints(IAccountRepository accountRepository, IConfiguration configuration, IDateTimeProvider dateTimeProvider)
	{
		_accountRepository = accountRepository;
		_configuration = configuration;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task Handle(NewAccountNotification notification, CancellationToken cancellationToken)
	{
		int defaultPoints = _configuration.GetValue<int>("Identity:defaultPoints");
		await _accountRepository.AddPointsAsync(notification.AccountId, defaultPoints, "Aktywacja konta!", _dateTimeProvider.Now);
	}
}
