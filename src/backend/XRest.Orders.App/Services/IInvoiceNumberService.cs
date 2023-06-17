namespace XRest.Orders.App.Services;

public interface IInvoiceNumberService
{
	ValueTask<string?> CreateAsync(int restaurantId);
}