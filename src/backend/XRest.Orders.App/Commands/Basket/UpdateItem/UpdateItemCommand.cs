using XRest.Orders.Contracts.Request.Basket;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Commands.Basket.UpdateItem;

public record UpdateItemCommand(string BasketKey, string ItemId, BasketItemSelectedProduct Product, string? Lang) : IRequest<BasketView>;