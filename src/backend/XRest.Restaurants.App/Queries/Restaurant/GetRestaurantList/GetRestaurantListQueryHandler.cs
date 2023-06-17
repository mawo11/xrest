using XRest.Restaurants.App.Services;
using XRest.Restaurants.Contracts;
using MediatR;

namespace XRest.Restaurants.App.Queries.Restaurant.GetRestaurantList;

public sealed class GetRestaurantListQueryHandler : IRequestHandler<GetRestaurantListQuery, GetRestaurantResponse>
{
	public IRestaurantCache _restaurantFactory;

	public GetRestaurantListQueryHandler(IRestaurantCache restaurantFactory)
	{
		_restaurantFactory = restaurantFactory;
	}

	public async Task<GetRestaurantResponse> Handle(GetRestaurantListQuery request, CancellationToken cancellationToken)
	{
		var items = (await _restaurantFactory.GetAllRestaurantsAsync())
												.Where(static x => x.IsPosCheckout)
												.OrderBy(static x => x.Name)
												.Select(x => new XRest.Restaurants.Contracts.Restaurant(
													x.Id,
													 x.Name,
													 x.Address,
													 -1,
													 x.RealizationTime,
													 x.MinOrder.ToString("C", CultureSettings.DefaultCultureInfo),
													 x.IsWorkingOnline(DateTime.Now),
													 x.Alias,
													 x.PostCode,
													 x.City,
													 x.GetOnlineFrom(),
													 x.GetOnlineTo()
													))
												.ToArray();

		return new GetRestaurantResponse(items);
	}
}
