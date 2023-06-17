using XRest.Shared.Infrastructure;
using XRest.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace XRest.Shared;

public static class ServiceExtensions
{
	public static void AddSharedServices(this IServiceCollection services)
	{
		services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
		services.AddSingleton<IMailRepository, MailRepository>();
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
	}
}
