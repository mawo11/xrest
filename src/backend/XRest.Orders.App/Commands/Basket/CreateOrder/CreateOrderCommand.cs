using XRest.Orders.Contracts.Request.Basket;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Commands.Basket.CreateOrder;

public record CreateOrderCommand(string BasketKey, CreateOrderRequest Request, int AccountId) : IRequest<CreateOrderResponse>;