using XRest.Consul;
using XRest.Identity.Contracts.Customers.Responses;
using XRest.Identity.Contracts.ExternalAuthenticate.Request;
using XRest.Identity.Contracts.ExternalAuthenticate.Responses;

namespace XRest.Identity.Clients;

public interface IExternalAuthenticateService
{
	ValueTask<ServiceCallResultData<ExternalLoginResponse?>> Login(ExternalAuthenticateLogonRequest request);

	ValueTask<ServiceCallResultData<ExternalProviderItem[]?>> GetActiveProviders(int clientType);

	ValueTask<ServiceCallResultData<LogonData?>> GetLogonData(string logonId);

	ValueTask<ServiceCallResultData<ExternalLoginResponse?>> ResumeLogin(ResumeLoginRequest request);
}
