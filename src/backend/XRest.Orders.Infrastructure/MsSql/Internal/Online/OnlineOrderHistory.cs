
using XRest.Orders.App.Domain;

namespace XRest.Orders.Infrastructure.MsSql.Internal.Online;
internal class OnlineOrderHistory
{
	public int Id { get; set; }

	public DateTime Created { get; set; }

	public string? Info { get; set; }

	public OrderStatus Status { get; set; }
}
