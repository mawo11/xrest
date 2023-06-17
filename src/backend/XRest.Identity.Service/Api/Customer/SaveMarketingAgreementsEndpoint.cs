using XRest.Authentication;
using XRest.Identity.App.Commands.Customers.SaveCustomerMakertingAgreements;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class SaveMarketingAgreementsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/customer/save-marketing-agreements", [Authorize(AuthenticationSchemes = SecuritySchemes.UserAccessScheme, Policy = PolicyTypes.UserAccessPolicy)] async (
		   HttpContext context,
		   [FromBody] IEnumerable<CustomerMarketingAgreement> items,
		   [FromServices] ISender sender) =>
	   {
		   var accountId = context.User.GetUserId();

		   return await sender.Send(new SaveCustomerMakertingAgreementsCommand(items, accountId));
	   });
	}
}
