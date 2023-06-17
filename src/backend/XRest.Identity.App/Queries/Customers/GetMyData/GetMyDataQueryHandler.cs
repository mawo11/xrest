using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Queries.Customers.GetMyData;

public sealed class GetMyDataQueryHandler : IRequestHandler<GetMyDataQuery, MyData>
{
	private readonly static MyData Error = new() { Success = false };

	private readonly IAccountRepository _accountRepository;

	public GetMyDataQueryHandler(IAccountRepository accountRepository)
	{
		_accountRepository = accountRepository;
	}

	public async Task<MyData> Handle(GetMyDataQuery request, CancellationToken cancellationToken)
	{
		var account = await _accountRepository.GetAccountByIdAsync(request.AccountId);

		if (account == null)
		{
			return Error;
		}

		return new MyData
		{
			Success = true,
			Birthdate = account.BirthDate,
			Firstname = account.Firstname,
			Lastname = account.Lastname,
			Phone = account.Phone
		};
	}
}
