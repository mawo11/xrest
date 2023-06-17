using XRest.Restaurants.App.Queries.Payment.GetPaymentInformation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Payment;

internal static class RestaurantPaymentInformationEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/payments/restaurant/{restaurantId}/payment-information", async (
			[FromRoute] int restaurantId,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetPaymentInformationQuery(restaurantId));
		});
	}
}
