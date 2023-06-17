using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Queries.Basket.GetBasketView;

public record GetBasketViewQuery(string BasketKey, string? Lang) : IRequest<BasketView>;