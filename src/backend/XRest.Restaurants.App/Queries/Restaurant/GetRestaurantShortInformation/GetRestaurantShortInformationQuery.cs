using XRest.Restaurants.Contracts;
using MediatR;

namespace XRest.Restaurants.App.Queries.Restaurant.GetRestaurantShortInformation;

public record GetRestaurantShortInformationQuery(int RestaurantId) : IRequest<GetRestaurantShortInfoByIdResponse>;