
using XRest.Orders.App.Domain;

namespace XRest.Orders.Infrastructure.MsSql.Internal.Online;
internal interface IOnlineOrderBuilder
{
	OnlineOrder Build(OnlineOrderTable onlineOrderTable, OnlineInvoice onlineInvoice, IEnumerable<OnlineOrderPositionTable> products);
}