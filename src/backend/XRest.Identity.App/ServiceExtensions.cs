using XRest.Identity.App.Services;
using Microsoft.Extensions.DependencyInjection;
using XRest.Shared.Services;

namespace XRest.Identity.App;

public static class ServiceExtensions
{
	public static void AddAppServices(this IServiceCollection services)
	{
		services.AddSingleton<IPasswordHasher, PasswordHasher>();
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
		services.AddSingleton<IUserTokenGenerator, UserTokenGenerator>();
	}
}
