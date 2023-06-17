using XRest.Restaurants.Contracts;
using MediatR;

namespace XRest.Restaurants.App.Commands.Restaurant.CalculateDeliveryPrice;

public record CalculateDeliveryPriceCommand(int RestaurantId, decimal OrderTotal, int TransportZoneId) : IRequest<CalculateDeliveryPriceResponse>;