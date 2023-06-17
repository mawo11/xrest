using XRest.Orders.Contracts.Request.Basket;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Commands.Basket.ChangeDeliveryType;

public record ChangeDeliveryTypeCommand(string BasketKey, DeliveryType DeliveryType) : IRequest<OperationStatus>;