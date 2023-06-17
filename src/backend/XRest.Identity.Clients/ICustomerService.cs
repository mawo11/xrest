using XRest.Consul;
using XRest.Identity.Contracts.Common;
using XRest.Identity.Contracts.Customers.Request;
using XRest.Identity.Contracts.Customers.Responses;

namespace XRest.Identity.Clients;

public interface ICustomerService
{
	ValueTask<ServiceCallResultData<LogonData?>> Login(LogonRequest request, CancellationToken cancellationToken = default);

	ValueTask<ServiceCallResult> RemoveAccount(string token, CancellationToken cancellationToken = default);

	ValueTask<ServiceCallResultData<NewAccountResponse?>> NewAccount(NewAccountRequest newAccount, CancellationToken cancellationToken = default);

	ValueTask<ServiceCallResultData<TokenData?>> RefreshToken(string token, CancellationToken cancellationToken = default);
}
