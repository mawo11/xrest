using XRest.Restaurants.App.Queries.Restaurant.GetInformationForOrdering;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Restaurant;

internal static class OrderingInformationEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/restaurant/{restaurantId}/ordering-information", async (
			[FromRoute] int restaurantId,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetInformationForOrderingQuery(restaurantId));
		});
	}
}
