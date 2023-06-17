using XRest.Orders.App.Queries.Basket.GetItemDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class ItemDetailsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/basket/{basketKey}/item/{itemId}/details", async (
			[FromRoute] string basketKey,
			[FromRoute] string itemId,
			[FromQuery] string? lang,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetItemDetailsQuery(basketKey, itemId, lang));
		});
	}
}
