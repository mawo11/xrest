using XRest.Orders.App.Extensions;
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Customers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Queries.Customers.GetMyOrders;

public sealed class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, MyOrdersResponse>
{
	private readonly static MyOrdersResponse Error = new(Contracts.Common.ApiIOperationStatus.Error, Array.Empty<MyOrder>());
	private readonly ILogger<GetMyOrdersQueryHandler> _logger;
	private readonly IOnlineOrderRepository _onlineOrderRepository;

	public GetMyOrdersQueryHandler(ILogger<GetMyOrdersQueryHandler> logger, IOnlineOrderRepository onlineOrderRepository)
	{
		_logger = logger;
		_onlineOrderRepository = onlineOrderRepository;
	}

	public async Task<MyOrdersResponse> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
	{
		try
		{
			_logger.LogInformation("GetMyOrdersQueryHandler => {AccountId}", request.AccountId);
			var orders = await _onlineOrderRepository.GetCustomerOrderShortInfoAsync(request.AccountId);

			var myOrders = orders
						.Select(x => new MyOrder
						{
							Id = x.Id,
							CreatedDate = x.CreateDate.ToString("yyyy-MM-dd HH:mm"),
							RestaurantName = x.RestaurantName,
							Status = x.Status.AsText(),
							Amount = x.Amount.ToString("C", CultureSettings.DefaultCultureInfo)
						})
						.ToArray();

			return new(Contracts.Common.ApiIOperationStatus.Ok, myOrders);
		}
		catch (Exception ex)
		{
			_logger.LogCritical(ex, "GetMyOrdersQueryHandler");
		}

		return Error;
	}
}
