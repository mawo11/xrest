using XRest.Orders.App.Queries.Menu.GetMenuGroupProductsForRestauantAndGroup;
using XRest.Orders.App.Queries.Menu.GetMenuGroupsForRestaurant;
using XRest.Orders.Contracts.Responses.Menu;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Menu;

internal static class RestuarantMenuEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/menu/restaurant/{restaurantId}/menu", async (
			[FromRoute] int restaurantId,
			[FromQuery] string? lang,
			[FromServices] ISender sender) =>
		{
			var groups = await sender.Send(new GetMenuGroupsForRestaurantQuery(restaurantId, lang));

			var products = groups.Length > 0 ?
				await sender.Send(new GetMenuGroupProductsForRestauantAndGroupQuery(restaurantId, groups[0].Id, lang))
				: Array.Empty<MenuProduct>();

			return new XRest.Orders.Contracts.Responses.Menu.Menu
			{
				Groups = groups,
				Products = products
			};
		});
	}
}
