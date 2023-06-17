using XRest.Orders.App.Domain;
using XRest.Orders.App.Resources;

namespace XRest.Orders.App.Extensions;

public static class OrderStatusExtension
{
	public static string? AsText(this OrderStatus status)
	{
		return OrderStatusStrings.ResourceManager.GetString(status.ToString());
	}

}
