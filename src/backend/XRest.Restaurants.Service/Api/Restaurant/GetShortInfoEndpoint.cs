using XRest.Restaurants.App.Queries.Restaurant.GetRestaurantShortInformation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Restaurant;

internal static class GetShortInfoEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/restaurant/{restaurantId}/short-info", async (
			[FromRoute] int restaurantId,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetRestaurantShortInformationQuery(restaurantId));
		});
	}
}
