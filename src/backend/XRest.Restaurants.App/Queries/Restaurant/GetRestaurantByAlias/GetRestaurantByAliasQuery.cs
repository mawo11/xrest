using XRest.Restaurants.Contracts;
using MediatR;

namespace XRest.Restaurants.App.Queries.Restaurant.GetRestaurantByAlias;

public record GetRestaurantByAliasQuery(string Alias) : IRequest<RestaurantOrderingData>;