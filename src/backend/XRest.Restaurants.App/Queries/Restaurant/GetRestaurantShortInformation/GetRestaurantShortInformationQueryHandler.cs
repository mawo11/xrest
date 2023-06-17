using XRest.Restaurants.App.Services;
using XRest.Restaurants.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Restaurants.App.Queries.Restaurant.GetRestaurantShortInformation;

public sealed class GetRestaurantShortInformationQueryHandler : IRequestHandler<GetRestaurantShortInformationQuery, GetRestaurantShortInfoByIdResponse>
{
	private readonly static GetRestaurantShortInfoByIdResponse Fail = new() { Success = false };

	private readonly IRestaurantCache _restaurantFactory;
	private readonly ILogger<GetRestaurantShortInformationQueryHandler> _logger;
	private readonly IRestaurantRepository _restaurantRepository;

	public GetRestaurantShortInformationQueryHandler(IRestaurantCache restaurantFactory,
		ILogger<GetRestaurantShortInformationQueryHandler> logger,
		IRestaurantRepository restaurantRepository)
	{
		_restaurantFactory = restaurantFactory;
		_logger = logger;
		_restaurantRepository = restaurantRepository;
	}

	public async Task<GetRestaurantShortInfoByIdResponse> Handle(GetRestaurantShortInformationQuery request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("GetRestaurantShortInformationQueryHandler => restId: {restId}", request.RestaurantId);

		var rest = await _restaurantFactory.GetByIdAsync(request.RestaurantId);
		if (rest == null)
		{
			return Fail;
		}

		var restOrderInfo = await _restaurantRepository.GetRestaurantOrderInformationByIdAsync(request.RestaurantId);
		if (restOrderInfo == null)
		{
			return Fail;
		}

		var result = new RestaurantShortInfo
		{
			Address = rest.Address,
			City = rest.City,
			Id = rest.Id,
			Name = rest.Name,
			PostCode = rest.PostCode,
			OrderDay = restOrderInfo.OrderDay
		};

		return new GetRestaurantShortInfoByIdResponse
		{
			Success = true,
			RestaurantShortInfo = result
		};
	}
}
