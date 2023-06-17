using XRest.Orders.App.Commands.Basket.DecraseItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class ItemDecraseEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/basket/{basketKey}/item/{itemId}/decrase", async (
			[FromRoute] string basketKey,
			[FromRoute] string itemId,
			[FromQuery] string? lang,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new DecraseItemCommand(basketKey, itemId, lang));
		});
	}
}
