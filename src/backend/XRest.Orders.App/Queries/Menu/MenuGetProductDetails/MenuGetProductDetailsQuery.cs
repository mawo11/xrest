using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Queries.Menu.MenuGetProductDetails;

public record MenuGetProductDetailsQuery(int ProductId, string? Lang) : IRequest<ProductDetails>;