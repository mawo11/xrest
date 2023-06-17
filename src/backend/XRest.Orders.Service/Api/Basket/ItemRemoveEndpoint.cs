using XRest.Orders.App.Commands.Basket.RemoveItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class ItemRemoveEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/basket/{basketKey}/item/{itemId}/remove", async (
			[FromRoute] string basketKey,
			[FromRoute] string itemId,
			[FromQuery] string? lang,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new RemoveItemCommand(basketKey, itemId, lang));
		});
	}
}
