using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Commands.Basket.AddProduct;

public sealed class AddProductCommandHandler : IRequestHandler<AddProductCommand, OperationStatus>
{
	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<AddProductCommandHandler> _logger;
	private readonly IReadOnlyProductRepository _readOnlyProductRepository;
	private readonly IFingerprintGenerator _fingerprintGenerator;

	public AddProductCommandHandler(IBasketStorage basketStorage,
		ILogger<AddProductCommandHandler> logger,
		IReadOnlyProductRepository readOnlyProductRepository,
		IFingerprintGenerator fingerprintGenerator)
	{
		_fingerprintGenerator = fingerprintGenerator;
		_readOnlyProductRepository = readOnlyProductRepository;
		_basketStorage = basketStorage;
		_logger = logger;
	}

	public Task<OperationStatus> Handle(AddProductCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("AddProductCommandHandler -> basketKey: {BasketKey} productId: {ProductId}",
			request.BasketKey,
			request.Product?.Id);

		if (string.IsNullOrEmpty(request.BasketKey))
		{
			_logger.LogError("brak basketkey in request");
			return Task.FromResult(OperationStatus.Expired);
		}

		var basketData = _basketStorage.GetByKey(request.BasketKey);
		if (basketData == null || basketData.IsLocked)
		{
			_logger.LogError("brak baskety data");
			return Task.FromResult(OperationStatus.Expired);
		}

		if (request.Product == null)
		{
			return Task.FromResult(OperationStatus.Error);
		}

		var readOnlyProduct = _readOnlyProductRepository.GetReadOnlyProductById(request.Product.ProductId);

		if (readOnlyProduct != null)
		{
			string itemId = _fingerprintGenerator.Generate(request.BasketKey, request.Product);
			request.Product.Id = itemId;
			basketData.Items.Add(new Domain.BasketItem(request.Product, readOnlyProduct, itemId, basketData));
		}

		return Task.FromResult(OperationStatus.Ok);
	}
}
