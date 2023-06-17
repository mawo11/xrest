
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Commands.Basket.RemoveAll;
public sealed class RemoveAllCommandHandler : IRequestHandler<RemoveAllCommand, OperationStatus>
{
	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<RemoveAllCommandHandler> _logger;

	public RemoveAllCommandHandler(IBasketStorage basketStorage, ILogger<RemoveAllCommandHandler> logger)
	{
		_basketStorage = basketStorage;
		_logger = logger;
	}

	public Task<OperationStatus> Handle(RemoveAllCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("RemoveAllCommandHandler -> basketKey: {BasketKey} ", request.BasketKey);
		if (string.IsNullOrEmpty(request.BasketKey))
		{
			return Task.FromResult(OperationStatus.Expired);
		}

		var basketData = _basketStorage.GetByKey(request.BasketKey);
		if (basketData == null || basketData.IsLocked)
		{
			return Task.FromResult(OperationStatus.Expired);
		}

		basketData.Items.Clear();

		return Task.FromResult(OperationStatus.Ok);
	}
}
