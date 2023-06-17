using XRest.Authentication;
using XRest.Identity.App.Commands.Customers.SaveDetails;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class UpdateDetailsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/customer/update-details", [Authorize(AuthenticationSchemes = SecuritySchemes.UserAccessScheme, Policy = PolicyTypes.UserAccessPolicy)] async (
		   HttpContext context,
		   [FromBody] MyData myData,
		   [FromServices] ISender sender) =>
	   {
		   var accountId = context.User.GetUserId();

		   return await sender.Send(new SaveDetailsCommand(accountId, myData));
	   });
	}
}
