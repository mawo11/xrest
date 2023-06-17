using XRest.Restaurants.App.Services;
using XRest.Restaurants.Infratructure.MsSql;
using Microsoft.Extensions.DependencyInjection;

namespace XRest.Restaurants.Infratructure;

public static class ServiceExtensions
{
	public static void AddInfrastrureServices(this IServiceCollection services)
	{
		services.AddSingleton<ITransportZonesRepository, TransportZonesRepository>();
		services.AddSingleton<IAddressRepository, AddressRepository>();
		services.AddSingleton<IRestaurantRepository, RestaurantRepository>();
		services.AddSingleton<IPaymentRepository, PaymentRepository>();
	}
}
