using XRest.Authentication;
using XRest.Identity.App.Queries.Customers.GetMyData;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class DetailsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/customer/details", [Authorize(AuthenticationSchemes = SecuritySchemes.UserAccessScheme, Policy = PolicyTypes.UserAccessPolicy)] async (
		   HttpContext context,
		   IHttpContextAccessor httpContextAccessor,
		   [FromServices] ISender sender) =>
	   {
		   var accountId = context.User.GetUserId();

		   return await sender.Send(new GetMyDataQuery(accountId));
	   });
	}
}