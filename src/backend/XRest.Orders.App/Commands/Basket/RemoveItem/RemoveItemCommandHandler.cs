
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Commands.Basket.RemoveItem;
public sealed class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, BasketView>
{
	private readonly static BasketView Empty = new();
	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<RemoveItemCommandHandler> _logger;
	private readonly IBasketViewGenerator _basketViewGenerator;

	public RemoveItemCommandHandler(
		IBasketStorage basketStorage,
		ILogger<RemoveItemCommandHandler> logger,
		IBasketViewGenerator basketViewGenerator)
	{
		_basketViewGenerator = basketViewGenerator;
		_basketStorage = basketStorage;
		_logger = logger;
	}

	public async Task<BasketView> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("RemoveItemCommandHandler -> basketKey: {BasketKey} ItemId: {ItemId} ",
			request.BasketKey,
			request.ItemId);
		if (string.IsNullOrEmpty(request.BasketKey))
		{
			return Empty;
		}

		var basketData = _basketStorage.GetByKey(request.BasketKey);
		if (basketData == null || basketData.IsLocked)
		{
			return Empty;
		}

		basketData.Items.RemoveAll(x => x.Id == request.ItemId);

		return await _basketViewGenerator.GenerateAsync(basketData, request.Lang);
	}
}
