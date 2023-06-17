using XRest.Orders.Contracts.Request.Basket;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace XRest.Orders.App.Commands.Basket.AddProduct;

public record AddProductCommand(string BasketKey, BasketItemSelectedProduct Product, string? Lang) : IRequest<OperationStatus>;