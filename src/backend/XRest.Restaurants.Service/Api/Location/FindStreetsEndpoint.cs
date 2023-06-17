using XRest.Restaurants.App.Queries.Location.GetStreetByCityAndStreet;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Payment;

internal static class FindStreetsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/location/find-streets", async (
			[FromQuery] string city,
			[FromQuery] string street,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetStreetByCityAndStreetQuery(city, street));
		});
	}
}
