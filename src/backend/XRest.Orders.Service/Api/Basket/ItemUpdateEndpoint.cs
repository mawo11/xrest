using XRest.Orders.App.Commands.Basket.UpdateItem;
using XRest.Orders.Contracts.Request.Basket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class ItemUpdateEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/basket/{basketKey}/item/{itemId}/update", async (
			[FromRoute] string basketKey,
			[FromRoute] string itemId,
			[FromQuery] string? lang,
			[FromBody] BasketItemSelectedProduct product,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new UpdateItemCommand(basketKey, itemId, product, lang));
		});
	}
}