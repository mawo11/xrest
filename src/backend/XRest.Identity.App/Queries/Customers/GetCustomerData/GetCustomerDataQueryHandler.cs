using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Common;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Queries.Customers.GetCustomerData;

public sealed class GetCustomerDataQueryHandler : IRequestHandler<GetCustomerDataQuery, ApiOperationResultData<CustomerData>>
{
	private readonly static ApiOperationResultData<CustomerData> Error = new(ApiOperationResultStatus.Error);

	private readonly IAccountRepository _accountRepository;

	public GetCustomerDataQueryHandler(IAccountRepository accountRepository)
	{
		_accountRepository = accountRepository;
	}

	public async Task<ApiOperationResultData<CustomerData>> Handle(GetCustomerDataQuery request, CancellationToken cancellationToken)
	{
		var account = await _accountRepository.GetAccountByIdAsync(request.CustomerId);

		if (account == null)
		{
			return Error;
		}

		return new ApiOperationResultData<CustomerData>(ApiOperationResultStatus.Ok, new CustomerData
		{
			Points = account.Points,
			Deactived = account.IsDeleted || account.Islocked
		});
	}
}
