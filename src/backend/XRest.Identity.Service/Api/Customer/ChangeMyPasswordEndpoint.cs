using XRest.Authentication;
using XRest.Identity.App.Commands.Customers.ChangeMyPassword;
using XRest.Identity.Contracts.Customers.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class ChangeMyPasswordEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/customer/change-my-password", [Authorize(AuthenticationSchemes = SecuritySchemes.UserAccessScheme, Policy = PolicyTypes.UserAccessPolicy)] async (
		   HttpContext context,
		   [FromBody] ChangeMyPasswordRequest request,
		   [FromServices] ISender sender) =>
		{
			var accountId = context.User.GetUserId();

			return await sender.Send(new ChangeMyPasswordCommand(accountId, request.Password));
		});
	}
}
