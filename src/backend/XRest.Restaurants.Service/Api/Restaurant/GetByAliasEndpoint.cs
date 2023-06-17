using XRest.Restaurants.App.Queries.Restaurant.GetRestaurantByAlias;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Restaurant;

internal static class GetByAliasEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/restaurant/get-by-alias", async (
			[FromQuery] string alias,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetRestaurantByAliasQuery(alias));
		});
	}
}
