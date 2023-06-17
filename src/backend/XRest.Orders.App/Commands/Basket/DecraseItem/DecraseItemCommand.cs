using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Commands.Basket.DecraseItem;

public record DecraseItemCommand(string BasketKey, string ItemId, string? Lang) : IRequest<BasketView>;
