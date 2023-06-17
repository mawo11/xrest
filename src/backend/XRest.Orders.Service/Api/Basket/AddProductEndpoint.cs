using XRest.Orders.App.Commands.Basket.AddProduct;
using XRest.Orders.Contracts.Request.Basket;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class AddProductEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/basket/{basketKey}/add-product", async (
			[FromRoute] string basketKey,
			[FromBody] BasketItemSelectedProduct product,
			[FromQuery] string? lang,
			[FromServices] ISender sender) =>
		{
			return new OperationResult
			{
				Status = await sender.Send(new AddProductCommand(basketKey, product, lang))
			};
		});
	}
}
