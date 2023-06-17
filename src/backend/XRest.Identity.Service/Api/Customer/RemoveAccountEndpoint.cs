using XRest.Authentication;
using XRest.Identity.App.Commands.Customers.DisableAccount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class RemoveAccountEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/customer/remove-account", [Authorize(AuthenticationSchemes = SecuritySchemes.UserAccessScheme, Policy = PolicyTypes.UserAccessPolicy)] async (
		   HttpContext context,
		   [FromServices] ISender sender) =>
		{
			var accountId = context.User.GetUserId();

			return await sender.Send(new DisableAccountCommand(accountId));
		});

	}
}