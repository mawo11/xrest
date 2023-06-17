using XRest.Restaurants.App.Queries.Restaurant.GetRestaurantWorkingStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Restaurant;

internal static class IsWorkingEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/restaurant/{restaurantId}/is-working", async (
			[FromRoute] int restaurantId,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetRestaurantWorkingStatusQuery(restaurantId));
		});
	}
}