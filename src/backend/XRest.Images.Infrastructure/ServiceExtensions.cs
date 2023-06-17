using XRest.Images.App.Services;
using XRest.Images.Infrastructure.MsSql;
using Microsoft.Extensions.DependencyInjection;

namespace XRest.Images.Infrastructure;

public static class ServiceExtensions
{
	public static void AddInfrastrureServices(this IServiceCollection services)
	{
		services.AddSingleton<IImageRepository, ImageRepository>();
	}
}
