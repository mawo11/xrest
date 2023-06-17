using XRest.Orders.App.Queries.Menu.GetMenuGroupProductsForRestauantAndGroup;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Menu;

internal static class RestuarantGroupProductsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/menu/restaurant/{restaurantId}/group/{groupId}/products", async (
			[FromRoute] int restaurantId,
			[FromRoute] int groupId,
			[FromQuery] string? lang,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetMenuGroupProductsForRestauantAndGroupQuery(restaurantId, groupId, lang));
		});
	}
}

