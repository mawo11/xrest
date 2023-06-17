using XRest.Orders.App.Domain;

namespace XRest.Orders.App.Services;

public interface IOnlineOrderInvoiceRepository
{
	ValueTask<bool> SaveInvoiceAsync(OnlineInvoice onlineInvoice);

	ValueTask<NewInvoiceNumberResult> GetNextInvoiceNumberAsync(int restaurantId, int year, int month);
}
