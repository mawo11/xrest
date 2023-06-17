using XRest.Restaurants.App.Services;
using XRest.Restaurants.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Restaurants.App.Queries.Restaurant.GetTerms;

public sealed class GetTermsQueryHandler : IRequestHandler<GetTermsQuery, RestaurantTermsResponse>
{
	private readonly static RestaurantTermsResponse Error = new(Contracts.Common.ApiIOperationStatus.Error, null);
	private readonly IRestaurantCache _restaurantFactory;
	private readonly ILogger<GetTermsQueryHandler> _logger;

	public GetTermsQueryHandler(IRestaurantCache restaurantFactory, ILogger<GetTermsQueryHandler> logger)
	{
		_restaurantFactory = restaurantFactory;
		_logger = logger;
	}

	public async Task<RestaurantTermsResponse> Handle(GetTermsQuery request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("GetTermsQueryHandler: {RestaurantId}", request.RestaurantId);

		try
		{
			string? terms = (await _restaurantFactory.GetByIdAsync(request.RestaurantId))?.Terms ?? null;
			if (!string.IsNullOrEmpty(terms))
			{
				return new RestaurantTermsResponse(Contracts.Common.ApiIOperationStatus.Ok, terms);
			}
		}
		catch (Exception ex)
		{
			_logger.LogCritical(ex, "GetTermsQueryHandler");
		}

		return Error;
	}

}
