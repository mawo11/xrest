using XRest.Orders.App.Domain;

namespace XRest.Orders.App.Services;

public interface IOnlineOrderRepository
{
	ValueTask<int> SaveAsync(NewOnlineOrder newOnlineOrder);

	ValueTask<bool> UpdatePaymentStatusAsync(int orderId, OrderStatus stats, bool payed, string paymentInfo, DateTime modifyDate);

	ValueTask<OrderPaymentInfo?> GetOrderPaymentInfoAsync(int orderId);

	ValueTask<OnlineOrderHeader?> GetOnlineOrderHeaderAsync(int orderId);

	ValueTask<bool> AddStatusToHistoryAsync(int orderId, OrderStatus stats, string info, DateTime modifyDate);

	ValueTask<CustomerOrderShortInfo[]> GetCustomerOrderShortInfoAsync(int accountId);
}
