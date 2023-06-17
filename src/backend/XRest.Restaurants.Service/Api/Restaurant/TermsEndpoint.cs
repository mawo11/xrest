using XRest.Restaurants.App.Queries.Restaurant.GetTerms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Restaurant;

internal static class TermsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/restaurant/{restaurantId}/terms", async (
			[FromRoute] int restaurantId,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetTermsQuery(restaurantId));
		});
	}
}
