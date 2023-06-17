using XRest.Identity.App.Domain;
using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Commands.Customers.NewAccount;

public sealed class NewAccountCommandHandler : IRequestHandler<NewAccountCommand, NewAccountResponse>
{
	private readonly static NewAccountResponse EmailExsits = new(NewAccountStatus.EmailExists);
	private readonly static NewAccountResponse Ok = new(NewAccountStatus.Ok);
	private readonly static NewAccountResponse Error = new(NewAccountStatus.Error);

	private readonly IAccountRepository _accountRepository;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IPublisher _publisher;

	public NewAccountCommandHandler(IAccountRepository accountRepository, IPasswordHasher passwordHasher, IPublisher publisher)
	{
		_accountRepository = accountRepository;
		_passwordHasher = passwordHasher;
		_publisher = publisher;
	}

	public async Task<NewAccountResponse> Handle(NewAccountCommand request, CancellationToken cancellationToken)
	{
		if (request.NewAccount == null || string.IsNullOrEmpty(request.NewAccount.Email) || string.IsNullOrEmpty(request.NewAccount.Password))
		{
			return Error;
		}

		NewAccountData newAccountData = new()
		{
			Email = request.NewAccount.Email,
			Terms = request.NewAccount.Terms,
			Firstname = request.NewAccount.Firstname,
			Lastname = request.NewAccount.Lastname,
			Marketing = request.NewAccount.Marketing,
			Password = _passwordHasher.HashPassword(request.NewAccount.Password)
		};

		var accountId = await _accountRepository.InsertNewAccountAsync(newAccountData);
		if (accountId == -1)
		{
			return EmailExsits;
		}

		NewAccountNotification newAccountNotification = new(accountId, newAccountData);
		await _publisher.Publish(newAccountNotification, cancellationToken);

		return Ok;
	}
}
