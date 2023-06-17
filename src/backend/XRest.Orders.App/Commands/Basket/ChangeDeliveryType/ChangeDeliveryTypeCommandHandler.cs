using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Commands.Basket.ChangeDeliveryType;

public sealed class ChangeDeliveryTypeCommandHandler : IRequestHandler<ChangeDeliveryTypeCommand, OperationStatus>
{
	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<ChangeDeliveryTypeCommandHandler> _logger;

	public ChangeDeliveryTypeCommandHandler(IBasketStorage basketStorage, ILogger<ChangeDeliveryTypeCommandHandler> logger)
	{
		_basketStorage = basketStorage;
		_logger = logger;
	}

	public Task<OperationStatus> Handle(ChangeDeliveryTypeCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("ChangeDeliveryTypeCommandHandler-> basketKey: {BasketKey} newType: {DeliveryType}",
			request.BasketKey,
			request.DeliveryType);

		if (string.IsNullOrEmpty(request.BasketKey))
		{
			return Task.FromResult(OperationStatus.Expired);
		}

		var basketData = _basketStorage.GetByKey(request.BasketKey);
		if (basketData == null || basketData.IsLocked)
		{
			return Task.FromResult(OperationStatus.Expired);
		}

		basketData.Type = (Domain.DeliveryType)request.DeliveryType;

		return Task.FromResult(OperationStatus.Ok);
	}
}
