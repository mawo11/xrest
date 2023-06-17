
using XRest.Orders.App.Commands.Basket.RemoveItem;
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Commands.Basket.DecraseItem;
public sealed class DecraseItemCommandHandler : IRequestHandler<DecraseItemCommand, BasketView>
{
	private readonly static BasketView Empty = new()
	{
		CanSubmit = false,
		Empty = true
	};

	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<RemoveItemCommandHandler> _logger;
	private readonly IBasketViewGenerator _basketViewGenerator;

	public DecraseItemCommandHandler(
		IBasketStorage basketStorage,
		ILogger<RemoveItemCommandHandler> logger,
		IBasketViewGenerator basketViewGenerator)
	{
		_basketViewGenerator = basketViewGenerator;
		_basketStorage = basketStorage;
		_logger = logger;
	}

	public async Task<BasketView> Handle(DecraseItemCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("DecraseItemCommandHandler -> basketKey: {BasketKey} ItemId: {ItemId}  ",
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

		var item = basketData.Items.Find(x => x.Id == request.ItemId);
		if (item != null)
		{
			basketData.Items.Remove(item);
		}

		return await _basketViewGenerator.GenerateAsync(basketData, request.Lang);
	}
}
