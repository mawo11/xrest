
using XRest.Orders.Contracts.Common;

namespace XRest.Orders.Contracts.Responses.Customers;
public class MyOrdersResponse
{
	public MyOrdersResponse(ApiIOperationStatus status, MyOrder[] orders)
	{
		Status = status;
		Orders = orders;
	}

	public ApiIOperationStatus Status { get; }

	public MyOrder[] Orders { get; }
}
