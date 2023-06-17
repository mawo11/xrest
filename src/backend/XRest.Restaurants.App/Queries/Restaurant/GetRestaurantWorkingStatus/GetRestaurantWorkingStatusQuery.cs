using XRest.Restaurants.Contracts;
using MediatR;

namespace XRest.Restaurants.App.Queries.Restaurant.GetRestaurantWorkingStatus;

public record GetRestaurantWorkingStatusQuery(int RestaurantId) : IRequest<RestaurantWorkingStatusResposne>;
