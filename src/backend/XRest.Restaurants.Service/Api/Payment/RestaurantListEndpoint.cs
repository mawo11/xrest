using XRest.Restaurants.App.Queries.Payment.GetPaymentsForOrdering;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Payment;

internal static class RestaurantListEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/payments/restaurant/{restaurantId}/list", async (
			[FromRoute] int restaurantId,
			[FromQuery] string? lang,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetPaymentsForOrderingQuery(restaurantId, lang ?? "pl"));
		});
	}
}
