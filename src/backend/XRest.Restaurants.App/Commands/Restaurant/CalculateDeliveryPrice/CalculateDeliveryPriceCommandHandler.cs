using XRest.Restaurants.App.Services;
using XRest.Restaurants.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Restaurants.App.Commands.Restaurant.CalculateDeliveryPrice;

public sealed class CalculateDeliveryPriceCommandHandler : IRequestHandler<CalculateDeliveryPriceCommand, CalculateDeliveryPriceResponse>
{
	private static readonly CalculateDeliveryPriceResponse Zero = new();
	private readonly ILogger<CalculateDeliveryPriceCommandHandler> _logger;
	private readonly ITransportZoneFinderService _transportZoneFinderService;

	public CalculateDeliveryPriceCommandHandler(ILogger<CalculateDeliveryPriceCommandHandler> logger, ITransportZoneFinderService transportZoneFinderService)
	{
		_logger = logger;
		_transportZoneFinderService = transportZoneFinderService;
	}

	public Task<CalculateDeliveryPriceResponse> Handle(CalculateDeliveryPriceCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("GetRestaurantWorkingStatusQueryHandler => restId: {rest}", request.RestaurantId);
		var info = _transportZoneFinderService.GetRestaurantTransportInfoById(request.TransportZoneId);
		if (info == null)
		{
			return Task.FromResult(Zero);
		}

		var result = new CalculateDeliveryPriceResponse
		{
			DeliveryPrice = info.Prices
				   .Where(x => x.FromPrice < request.OrderTotal)
				   .OrderByDescending(x => x.FromPrice)
				   .FirstOrDefault()?.DeliveryPrice ?? 0
		};

		return Task.FromResult(result);
	}
}
