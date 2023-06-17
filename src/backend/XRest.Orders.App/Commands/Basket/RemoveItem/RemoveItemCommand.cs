using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Commands.Basket.RemoveItem;

public record RemoveItemCommand(string BasketKey, string ItemId, string? Lang) : IRequest<BasketView>;