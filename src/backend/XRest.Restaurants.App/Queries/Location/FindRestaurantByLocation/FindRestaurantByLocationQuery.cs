using XRest.Restaurants.Contracts;
using MediatR;

namespace XRest.Restaurants.App.Queries.FindRestaurantByLocation;

public record FindRestaurantByLocationQuery(string City, string Street, string StreetNumber, bool IsPosCheckout) : IRequest<RestaurantOrderingData>;
