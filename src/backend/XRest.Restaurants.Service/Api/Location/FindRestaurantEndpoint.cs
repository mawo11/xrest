using XRest.Restaurants.App.Queries.FindRestaurantByLocation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Payment;

internal static class FindRestaurantEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/location/find-restaurant", async (
			[FromQuery] string city,
			[FromQuery] string street,
			[FromQuery] string streetNumber,
			[FromQuery] bool isPosCheckout,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new FindRestaurantByLocationQuery(city, street, streetNumber, isPosCheckout));
		});
	}
}
