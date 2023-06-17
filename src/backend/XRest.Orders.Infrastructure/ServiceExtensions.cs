using XRest.Orders.App.Services;
using XRest.Orders.Infrastructure.MsSql;
using Microsoft.Extensions.DependencyInjection;

namespace XRest.Orders.Infrastructure;

public static class ServiceExtensions
{
	public static void AddInfrastrureServices(this IServiceCollection services)
	{
		services.AddSingleton<IReadOnlyProductRepository, ReadOnlyProductRepository>();
		services.AddSingleton<IOnlineOrderInvoiceRepository, OnlineOrderInvoiceRepository>();
		services.AddSingleton<IOnlineOrderRepository, OnlineOrderRepository>();
		services.AddSingleton<IFavoriteAddressRepository, FavoriteAddressRepository>();
		services.AddSingleton<IUndeliveredReasonRepository, UndeliveredReasonRepository>();
		services.AddSingleton<IPaymentRepository, PaymentRepository>();
	}
}
