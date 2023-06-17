using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Queries.Basket.GetBasketView;

public sealed class GetBasketViewQueryHandler : IRequestHandler<GetBasketViewQuery, BasketView>
{
	private readonly static BasketView Empty = new()
	{
		CanSubmit = false,
		Empty = true
	};

	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<GetBasketViewQueryHandler> _logger;
	private readonly IBasketViewGenerator _basketViewGenerator;

	public GetBasketViewQueryHandler(
		IBasketStorage basketStorage,
		ILogger<GetBasketViewQueryHandler> logger,
		IBasketViewGenerator basketViewGenerator)
	{
		_basketViewGenerator = basketViewGenerator;
		_basketStorage = basketStorage;
		_logger = logger;
	}

	public async Task<BasketView> Handle(GetBasketViewQuery request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("GetBasketViewQueryHandler -> basketKey: {BasketKey}", request.BasketKey);
		if (string.IsNullOrEmpty(request.BasketKey))
		{
			return Empty;
		}

		var basketData = _basketStorage.GetByKey(request.BasketKey);
		if (basketData == null)
		{
			return Empty;
		}

		return await _basketViewGenerator.GenerateAsync(basketData, request.Lang);
	}
}
