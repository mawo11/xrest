using MediatR;

namespace XRest.Identity.App.Domain;

public class NewAccountNotification : INotification
{
	public NewAccountNotification(int accountId, NewAccountData newAccountData)
	{
		AccountId = accountId;
		NewAccountData = newAccountData;
	}

	public int AccountId { get; }

	public NewAccountData NewAccountData { get; }
}
