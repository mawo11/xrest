using MediatR;

namespace XRest.Restaurants.App.Queries.Location.GetStreetByCityAndStreet;

public record GetStreetByCityAndStreetQuery(string City, string Street) : IRequest<string[]>;