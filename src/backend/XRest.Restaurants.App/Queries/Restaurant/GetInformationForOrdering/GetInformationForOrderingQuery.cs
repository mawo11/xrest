using XRest.Restaurants.Contracts;
using MediatR;

namespace XRest.Restaurants.App.Queries.Restaurant.GetInformationForOrdering;

public record GetInformationForOrderingQuery(int RestaurantId) : IRequest<RestaurantOrderInformation>;