using MediatR;

namespace XRest.Orders.App.Domain;

public class NewOnlineOrderCreatedNotification : INotification
{
	public NewOnlineOrderCreatedNotification(int orderId, NewOnlineOrder newOnlineOrder)
	{
		OrderId = orderId;
		NewOnlineOrder = newOnlineOrder;
	}

	public int OrderId { get; }

	public NewOnlineOrder NewOnlineOrder { get; }
}
