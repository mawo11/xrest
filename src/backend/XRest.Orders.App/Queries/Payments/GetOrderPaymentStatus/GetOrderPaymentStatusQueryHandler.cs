using XRest.Orders.App.Domain;
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Payments;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Queries.Payments.GetOrderPaymentStatus;

public sealed class GetOrderPaymentStatusQueryHandler : IRequestHandler<GetOrderPaymentStatusQuery, OrderPaymentStatus>
{
	private readonly static OrderPaymentStatus Ok = new(PaymentStatus.Ok);
	private readonly static OrderPaymentStatus Error = new(PaymentStatus.Error);
	private readonly static OrderPaymentStatus Waiting = new(PaymentStatus.Waiting);


	private readonly IOnlineOrderRepository _onlineOrderRepository;
	private readonly ILogger<GetOrderPaymentStatusQueryHandler> _logger;

	public GetOrderPaymentStatusQueryHandler(IOnlineOrderRepository onlineOrderRepository, ILogger<GetOrderPaymentStatusQueryHandler> logger)
	{
		_onlineOrderRepository = onlineOrderRepository;
		_logger = logger;
	}

	public async Task<OrderPaymentStatus> Handle(GetOrderPaymentStatusQuery request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("GetOrderPaymentStatusQueryHandler -> request id= {OrderId}", request.OrderId);
		OrderPaymentInfo? result = await _onlineOrderRepository.GetOrderPaymentInfoAsync(request.OrderId);

		if (result == null)
		{
			return Error;
		}

		switch (result.Status)
		{
			case OrderStatus.PaymentWaiting:
				return Waiting;
			case OrderStatus.Created:
				return Ok;
			default:
				return Error;
		}
	}
}
