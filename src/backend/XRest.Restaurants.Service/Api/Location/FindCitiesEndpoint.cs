using XRest.Restaurants.App.Queries.Location.GetCitiesByName;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Payment;

internal static class FindCitiesEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/location/find-cities", async (
			[FromQuery] string city,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetCitiesByNameQuery(city));
		});
	}
}
