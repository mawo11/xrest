using XRest.Orders.App.Commands.Basket.RemoveAll;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class RemoveAllEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/basket/{basketKey}/remove-all", async (
			[FromRoute] string basketKey,
			[FromServices] ISender sender) =>
		{
			return new OperationResult
			{
				Status = await sender.Send(new RemoveAllCommand(basketKey))
			};
		});
	}
}