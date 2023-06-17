
using XRest.Orders.App.Domain;
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Commands.Basket.Init;
public sealed class InitCommandHandler : IRequestHandler<InitCommand, BasketInitResponse>
{
	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<InitCommandHandler> _logger;
	private readonly IConfiguration _configuration;
	private readonly IReadOnlyProductRepository _readOnlyProductRepository;

	public InitCommandHandler(IBasketStorage basketStorage,
		ILogger<InitCommandHandler> logger,
		IConfiguration configuration,
		IReadOnlyProductRepository readOnlyProductRepository)
	{
		_readOnlyProductRepository = readOnlyProductRepository;
		_configuration = configuration;
		_basketStorage = basketStorage;
		_logger = logger;
	}

	public Task<BasketInitResponse> Handle(InitCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Basket Init -> restId: {RestaurantId} zoneId: {TransportZoneId} type: {Type}",
			request.RestaurantId,
			request.TransportZoneId,
			request.Type);

		if (!string.IsNullOrEmpty(request.BasketKey) && _basketStorage.GetByKey(request.BasketKey) != null)
		{
			return Task.FromResult(new BasketInitResponse(request.BasketKey));
		}

		var config = NLog.LogManager.Configuration;

		BasketData data = new()
		{
			BasketKey = Guid.NewGuid().ToString(),
			LastActiviy = DateTime.Now,
			RestaurantId = request.RestaurantId,
			TransportZoneId = request.TransportZoneId,
			Type = (DeliveryType)request.Type
		};

		int bagId = _configuration.GetValue("Orders:BagId", 0);
		if (bagId != 0)
		{
			data.BagProduct = _readOnlyProductRepository.GetReadOnlyProductById(bagId);
		}

		_basketStorage.Add(data);

		return Task.FromResult(new BasketInitResponse(data.BasketKey));
	}
}
