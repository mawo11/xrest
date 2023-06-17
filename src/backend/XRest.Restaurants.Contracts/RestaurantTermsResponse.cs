using XRest.Restaurants.Contracts.Common;

namespace XRest.Restaurants.Contracts;

public class RestaurantTermsResponse
{
	public RestaurantTermsResponse(ApiIOperationStatus status, string? text)
	{
		Status = status;
		Text = text;
	}

	public ApiIOperationStatus Status { get; }

	public string? Text { get; }
}
