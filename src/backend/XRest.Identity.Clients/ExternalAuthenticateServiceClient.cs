using XRest.Consul;
using XRest.Identity.Contracts;
using XRest.Identity.Contracts.Customers.Responses;
using XRest.Identity.Contracts.ExternalAuthenticate.Request;
using XRest.Identity.Contracts.ExternalAuthenticate.Responses;
using Microsoft.Extensions.Logging;

namespace XRest.Identity.Clients;

public class ExternalAuthenticateServiceClient : ServiceClient,
	IExternalAuthenticateService
{
	public ExternalAuthenticateServiceClient(IConsulServiceDiscovery consulServiceDiscovery,
		HttpClient httpClient,
		ILogger<ServiceClient> logger) : base(IdentityServiceName.Name, consulServiceDiscovery, httpClient, logger)
	{
	}

	public async ValueTask<ServiceCallResultData<ExternalLoginResponse?>> Login(ExternalAuthenticateLogonRequest request)
	{
		return await SendAsync<ExternalLoginResponse>(
			"/external/authenticate/login",
			HttpMethod.Post,
			request,
			null,
			CancellationToken.None);
	}

	public async ValueTask<ServiceCallResultData<ExternalProviderItem[]?>> GetActiveProviders(int clientType)
	{
		return await SendAsync<ExternalProviderItem[]>(
		   $"/external/authenticate/platform/{clientType}/active-providers",
			HttpMethod.Get,
			null,
			null,
			CancellationToken.None);
	}

	public async ValueTask<ServiceCallResultData<LogonData?>> GetLogonData(string logonId)
	{
		return await SendAsync<LogonData?>(
		   $"/external/authenticate/{logonId}/logon-data",
			HttpMethod.Post,
			null,
			null,
			CancellationToken.None);
	}

	public async ValueTask<ServiceCallResultData<ExternalLoginResponse?>> ResumeLogin(ResumeLoginRequest request)
	{
		return await SendAsync<ExternalLoginResponse>(
		  "/external/authenticate/resume-login",
		  HttpMethod.Post,
		  request,
		  null,
		  CancellationToken.None);
	}
}
