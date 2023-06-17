using XRest.Restaurants.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace XRest.Restaurants.App;

public static class ServiceExtensions
{
	public static void AddAppServices(this IServiceCollection services)
	{
		services.AddSingleton<ILocationFinderService, LocationFinderService>();
		services.AddSingleton<ITransportZoneFinderService, TransportZoneFinderService>();
		services.AddSingleton<IRestaurantCache, RestaurantCache>();
	}
}
