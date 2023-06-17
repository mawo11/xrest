using XRest.Restaurants.App.Services;
using MediatR;

namespace XRest.Restaurants.App.Queries.Location.GetStreetByCityAndStreet;

public sealed class GetStreetByCityAndStreetQueryHandler : IRequestHandler<GetStreetByCityAndStreetQuery, string[]>
{
	private ILocationFinderService _locationFinderService;

	public GetStreetByCityAndStreetQueryHandler(ILocationFinderService locationFinderService)
	{
		_locationFinderService = locationFinderService;
	}

	public Task<string[]> Handle(GetStreetByCityAndStreetQuery request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_locationFinderService.FindStreets(request.City, request.Street));
	}
}
