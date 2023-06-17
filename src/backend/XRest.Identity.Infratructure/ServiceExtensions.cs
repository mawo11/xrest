using XRest.Identity.App.Services;
using XRest.Identity.Infratructure.MsSql;
using Microsoft.Extensions.DependencyInjection;

namespace XRest.Identity.Infratructure;

public static class ServiceExtensions
{
	public static void AddInfrastrureServices(this IServiceCollection services)
	{
		services.AddSingleton<IMarketingAgreementRepository, MarketingAgreementRepository>();
		services.AddSingleton<IAccountRepository, AccountRepository>();
		services.AddSingleton<ITokenRepository, TokenRepository>();
		services.AddSingleton<IAccountOperationRepository, AccountOperationRepository>();
	}
}
