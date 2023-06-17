using XRest.Orders.App.Queries.Menu.GetMenuGroupsForRestaurant;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Menu;

internal static class RestuarantGroupsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/menu/restaurant/{restaurantId}/groups", async (
			[FromRoute] int restaurantId,
			[FromQuery] string? lang,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetMenuGroupsForRestaurantQuery(restaurantId, lang));
		});
	}
}
