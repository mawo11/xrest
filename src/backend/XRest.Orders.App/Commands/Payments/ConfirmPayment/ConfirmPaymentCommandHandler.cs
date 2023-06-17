using XRest.Orders.App.Domain;
using XRest.Orders.App.Services;
using XRest.Shared.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using XRest.Shared.Services;

namespace XRest.Orders.App.Commands.Payments.ConfirmPayment;

public sealed class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, string>
{
	private readonly IPaymentService _paymentService;
	private readonly ILogger<ConfirmPaymentCommandHandler> _logger;
	private readonly IOnlineOrderRepository _onlineOrderRepository;
	private readonly IDateTimeProvider _dateTimeProvider;

	public ConfirmPaymentCommandHandler(IPaymentService paymentService,
		ILogger<ConfirmPaymentCommandHandler> logger,
		IOnlineOrderRepository onlineOrderRepository,
		IDateTimeProvider dateTimeProvider)
	{
		_paymentService = paymentService;
		_logger = logger;
		_onlineOrderRepository = onlineOrderRepository;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<string> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
	{
		if (!(int.TryParse(request.OrderId, out int orderId) && int.TryParse(request.PaymentOrderId, out int paymentOrderId)))
		{
			_logger.LogError("ConfirmOrder  ==> nie można pobrać danych do weryfikacji {data} ", CustomJsonSerializer.Serialize(request));
			return "ERROR";
		}

		_logger.LogInformation("ConfirmOrder ==> OrderId: {orderId} PaymentOrderId: {paymentOrderId} ",
			orderId,
			paymentOrderId);

		PaymentInfo paymentInfo = new()
		{
			Created = _dateTimeProvider.Now,
			OrderId = paymentOrderId,
			Method = request.Method,
			Statement = request.Statement,
			Type = "przelewy24"
		};

		var orderHeader = await _onlineOrderRepository.GetOnlineOrderHeaderAsync(orderId);
		if (orderHeader != null && orderHeader.Status == OrderStatus.PaymentWaiting)
		{
			await _onlineOrderRepository.AddStatusToHistoryAsync(orderId, orderHeader.Status, orderHeader.StatusInfo ?? string.Empty, orderHeader.ModifyDate);

			await _onlineOrderRepository.UpdatePaymentStatusAsync(orderId,
				OrderStatus.Created,
				true,
				CustomJsonSerializer.Serialize(paymentInfo), _dateTimeProvider.Now);

			await _paymentService.ConfirmPaymentAsync(orderId, paymentOrderId, orderHeader.Amount, orderHeader.RestaurantId);
		}

		return "OK";
	}
}
