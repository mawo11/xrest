using XRest.Orders.Contracts.Request.Basket;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Commands.Basket.Init;

public record InitCommand(int RestaurantId, int TransportZoneId, DeliveryType Type, string BasketKey, OrderSource Source = OrderSource.CallCenter) : IRequest<BasketInitResponse>;