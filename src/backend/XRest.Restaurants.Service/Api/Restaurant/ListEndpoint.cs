using XRest.Restaurants.App.Queries.Restaurant.GetRestaurantList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Restaurant;

internal static class ListEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/restaurant/list", async (
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetRestaurantListQuery());
		});
	}
}