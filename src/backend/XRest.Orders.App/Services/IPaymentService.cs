using XRest.Orders.App.Domain;

namespace XRest.Orders.App.Services;

public interface IPaymentService
{
	ValueTask<RegisterPaymentResult> RegisterPaymentAsync(int orderId, NewOnlineOrder newOnlineOrder);

	ValueTask ConfirmPaymentAsync(int orderId, int paymentyOrderId, decimal amount, int restaurantId);
}