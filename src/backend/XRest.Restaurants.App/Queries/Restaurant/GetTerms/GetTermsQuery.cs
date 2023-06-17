using XRest.Restaurants.Contracts;
using MediatR;

namespace XRest.Restaurants.App.Queries.Restaurant.GetTerms;

public record GetTermsQuery(int RestaurantId) : IRequest<RestaurantTermsResponse>;