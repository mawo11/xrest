using XRest.Orders.App.Commands.Basket.UpdateItemNote;
using XRest.Orders.Contracts.Request.Basket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class UpdateNoteEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/basket/{basketKey}/item/{itemId}/update-note", async (
			[FromRoute] string basketKey,
			[FromRoute] string itemId,
			[FromQuery] string? lang,
			[FromBody] UpdateNoteRequest request,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new UpdateItemNoteCommand(basketKey, itemId, request.Note, lang));
		});
	}
}