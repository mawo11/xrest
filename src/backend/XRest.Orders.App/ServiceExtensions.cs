using XRest.Orders.App.Services;
using XRest.Orders.App.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace XRest.Orders.App;

public static class ServiceExtensions
{
	public static void AddAppServices(this IServiceCollection services)
	{
		services.AddSingleton<IFingerprintGenerator, FingerprintGenerator>();
		services.AddSingleton<IBasketStorage, InMemoryBasketStorage>();
		services.AddSingleton<IBasketViewGenerator, BasketViewGenerator>();
		services.AddSingleton<IMenuProductDetailsCreator, MenuProductDetailsCreator>();
		services.AddSingleton<IOrderCreatorValidator, NipValidator>();
		services.AddSingleton<IInvoiceNumberService, InvoiceNumberService>();
		services.AddSingleton<IOrderTotalCalculateService, OrderTotalCalculateService>();
		services.AddSingleton<IPaymentService, PaymentService>();
	}
}
