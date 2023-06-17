
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Commands.Basket.UpdateItem;
public sealed class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, BasketView>
{
	private readonly static BasketView Empty = new();

	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<UpdateItemCommandHandler> _logger;
	private readonly IFingerprintGenerator _fingerprintGenerator;
	private readonly IBasketViewGenerator _basketViewGenerator;
	private readonly IReadOnlyProductRepository _readOnlyProductRepository;

	public UpdateItemCommandHandler(
		IBasketStorage basketStorage,
		ILogger<UpdateItemCommandHandler> logger,
		IFingerprintGenerator fingerprintGenerator,
		IBasketViewGenerator basketViewGenerator,
		IReadOnlyProductRepository readOnlyProductRepository)
	{
		_readOnlyProductRepository = readOnlyProductRepository;
		_basketViewGenerator = basketViewGenerator;
		_fingerprintGenerator = fingerprintGenerator;
		_basketStorage = basketStorage;
		_logger = logger;
	}

	public async Task<BasketView> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("UpdateItemCommandHandler -> basketKey: {BasketKey} ItemId: {ItemId} productId: {ProductId}",
			request.BasketKey,
			request.ItemId,
			request.Product.Id);
		if (string.IsNullOrEmpty(request.BasketKey))
		{
			return Empty;
		}

		var basketData = _basketStorage.GetByKey(request.BasketKey);
		if (basketData == null || basketData.IsLocked)
		{
			return Empty;
		}

		string tempId = _fingerprintGenerator.Generate(request.BasketKey, request.Product);
		if (tempId != request.ItemId)
		{
			var item = basketData.Items.Find(x => x.Id == request.ItemId);
			if (item != null)
			{
				basketData.Items.Remove(item);
			}

			var readOnlyProduct = _readOnlyProductRepository.GetReadOnlyProductById(request.Product.ProductId);
			if (readOnlyProduct != null)
			{
				string itemId = _fingerprintGenerator.Generate(request.BasketKey, request.Product);
				basketData.Items.Add(new Domain.BasketItem(request.Product, readOnlyProduct, itemId, basketData));
			}
		}

		return await _basketViewGenerator.GenerateAsync(basketData, request.Lang);
	}
}
