using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Commands.Basket.RemoveAll;

public record RemoveAllCommand(string BasketKey): IRequest<OperationStatus>;