using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Queries.Basket.GetItemDetails;

public record GetItemDetailsQuery(string BasketKey, string ItemId, string? Lang) : IRequest<ProductDetails>;