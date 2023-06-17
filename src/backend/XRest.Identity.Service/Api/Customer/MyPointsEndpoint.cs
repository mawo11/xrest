using XRest.Authentication;
using XRest.Identity.App.Queries.Customers.GetCustomerPointHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class MyPointsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/customer/my-points", [Authorize(AuthenticationSchemes = SecuritySchemes.UserAccessScheme, Policy = PolicyTypes.UserAccessPolicy)] async (
		   HttpContext context,
		   [FromServices] ISender sender) =>
	   {
		   var accountId = context.User.GetUserId();

		   return await sender.Send(new GetCustomerPointHistoryQuery(accountId));
	   });
	}
}
