using XRest.Consul;
using XRest.Identity.Contracts;
using XRest.Identity.Contracts.Common;
using XRest.Identity.Contracts.Customers.Request;
using XRest.Identity.Contracts.Customers.Responses;
using Microsoft.Extensions.Logging;

namespace XRest.Identity.Clients;

public sealed class CustomerServiceClient : ServiceClient, ICustomerService
{
	public CustomerServiceClient(IConsulServiceDiscovery consulServiceDiscovery,
		HttpClient httpClient,
		ILogger<ServiceClient> logger) : base(IdentityServiceName.Name, consulServiceDiscovery, httpClient, logger)
	{
	}

	public async ValueTask<ServiceCallResultData<LogonData?>> Login(LogonRequest request, CancellationToken cancellationToken)
	{
		return await SendAsync<LogonData?>(
			"/customer/logon",
			HttpMethod.Post,
			request,
			null,
			cancellationToken);
	}

	public async ValueTask<ServiceCallResultData<TokenData?>> RefreshToken(string token, CancellationToken cancellationToken)
	{
		return await SendAsync<TokenData?>(
			"/customer/refresh-token",
			HttpMethod.Post,
			null,
			token,
			cancellationToken);
	}

	public async ValueTask<ServiceCallResult> RemoveAccount(string token, CancellationToken cancellationToken)
	{
		var result = await SendAsync<ApiOperationResult?>(
			"/customer/remove-account",
			HttpMethod.Post,
			null,
			token,
			cancellationToken);
		if (result.Status == ServiceCallStatus.Ok)
		{
			return ServiceCallResult.Ok;
		}

		return ServiceCallResult.Error;
	}

	public async ValueTask<ServiceCallResultData<NewAccountResponse?>> NewAccount(NewAccountRequest newAccount, CancellationToken cancellationToken)
	{
		return await SendAsync<NewAccountResponse?>(
		  "/customer/new-account",
		  HttpMethod.Post,
		  newAccount,
		  null,
		  cancellationToken);
	}
}