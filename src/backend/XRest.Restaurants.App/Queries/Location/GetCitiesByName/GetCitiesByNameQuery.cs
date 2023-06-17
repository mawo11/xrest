using MediatR;

namespace XRest.Restaurants.App.Queries.Location.GetCitiesByName;

public record GetCitiesByNameQuery(string City) : IRequest<string[]>;