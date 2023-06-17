
using XRest.Orders.App.Domain;
using System.Data;

namespace XRest.Orders.Infrastructure.MsSql.Internal.Online;
internal interface IOnlineOrderSerializator
{
	(DataTable header, DataTable rows) Serialize(NewOnlineOrder newOnlineOrder);
}