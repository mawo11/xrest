using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Commands.Basket.IncraseItem;

public record IncraseItemCommand(string BasketKey, string ItemId, string? Lang) : IRequest<BasketView>;