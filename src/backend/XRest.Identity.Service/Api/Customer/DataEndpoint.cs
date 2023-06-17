using XRest.Identity.App.Queries.Customers.GetCustomerData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class DataEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/customer/{customerId}/data", async (
		   [FromRoute] int customerId,
		   [FromServices] ISender sender) =>
	   {
		   return await sender.Send(new GetCustomerDataQuery(customerId));
	   });
	}
}
