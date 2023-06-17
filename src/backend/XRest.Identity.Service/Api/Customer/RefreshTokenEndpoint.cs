using XRest.Authentication;
using XRest.Identity.App.Commands.Customers.RefreshToken;
using XRest.Identity.Contracts.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class RefreshTokenEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/customer/refresh-token", [Authorize(AuthenticationSchemes = SecuritySchemes.RefreshScheme)] async (
		   HttpContext context,
		   [FromServices] ISender sender) =>
	   {
		   var accountId = context.User.GetUserId();
		   var token = context.User.FindFirst(CustomClaims.RefreshToken)?.Value;

		   if (string.IsNullOrEmpty(token))
		   {
			   return Results.Unauthorized();
		   }

		   var result = await sender.Send(new RefreshTokenCommand(accountId, token));

		   if (result == TokenData.InvalidToken)
		   {
			   return Results.Unauthorized();
		   }

		   return Results.Ok(result);
	   }).Produces<TokenData>(StatusCodes.Status200OK);
	}
}