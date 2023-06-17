using XRest.Restaurants.Contracts;
using MediatR;

namespace XRest.Restaurants.App.Queries.Restaurant.GetRestaurantList;

public record GetRestaurantListQuery : IRequest<GetRestaurantResponse>;
