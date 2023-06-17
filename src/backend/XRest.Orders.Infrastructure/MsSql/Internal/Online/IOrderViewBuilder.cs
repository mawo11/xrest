
using XRest.Orders.App.Domain;
using XRest.Orders.Infrastructure.MsSql.Internal.Online;

namespace XRest.Orders.App.Services;
internal interface IOrderViewBuilder
{
	OrderView Build(OnlineOrderTable onlineOrderTable, OnlineInvoice onlineInvoice, IEnumerable<OnlineOrderPositionTable> products, IEnumerable<OnlineOrderHistory> orderHistory, IEnumerable<UndeliveredReason> undeliveredReasons);
}