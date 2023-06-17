using XRest.Restaurants.App.Services;
using MediatR;

namespace XRest.Restaurants.App.Queries.Location.GetCitiesByName;

public sealed class GetCitiesByNameQueryHandler : IRequestHandler<GetCitiesByNameQuery, string[]>
{
	private readonly ILocationFinderService _locationFinderService;

	public GetCitiesByNameQueryHandler(ILocationFinderService locationFinderService)
	{
		_locationFinderService = locationFinderService;
	}

	public Task<string[]> Handle(GetCitiesByNameQuery request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_locationFinderService.FindCities(request.City));
	}
}
