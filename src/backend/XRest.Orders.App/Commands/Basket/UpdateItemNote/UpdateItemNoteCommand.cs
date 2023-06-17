using XRest.Orders.Contracts.Responses.Basket;
using MediatR;

namespace XRest.Orders.App.Commands.Basket.UpdateItemNote;

public record UpdateItemNoteCommand(string BasketKey, string ItemId, string? Note, string? Lang) : IRequest<BasketView>;