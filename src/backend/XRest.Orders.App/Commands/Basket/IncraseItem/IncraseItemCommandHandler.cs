
using XRest.Orders.App.Commands.Basket.RemoveItem;
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Commands.Basket.IncraseItem;
public sealed class IncraseItemCommandHandler : IRequestHandler<IncraseItemCommand, BasketView>
{
	private readonly static BasketView Empty = new()
	{
		CanSubmit = false,
		Empty = true
	};

	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<RemoveItemCommandHandler> _logger;
	private readonly IBasketViewGenerator _basketViewGenerator;
	private readonly IFingerprintGenerator _fingerprintGenerator;

	public IncraseItemCommandHandler(
		IBasketStorage basketStorage,
		ILogger<RemoveItemCommandHandler> logger,
		IBasketViewGenerator basketViewGenerator,
		IFingerprintGenerator fingerprintGenerator)
	{
		_fingerprintGenerator = fingerprintGenerator;
		_basketViewGenerator = basketViewGenerator;
		_basketStorage = basketStorage;
		_logger = logger;
	}

	public async Task<BasketView> Handle(IncraseItemCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("IncraseItemCommandHandler -> basketKey: {BasketKey} ItemId: {ItemId} ",
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
			var newItem = item.Clone();
			string newId = _fingerprintGenerator.Generate(request.BasketKey, newItem.SelectedProducts);
			newItem.AssignNewId(newId);

			basketData.Items.Add(newItem);
		}

		return await _basketViewGenerator.GenerateAsync(basketData, request.Lang);
	}
}
