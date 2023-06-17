using MediatR;
using XRest.Identity.Contracts.ExternalAuthenticate.Responses;
using Microsoft.Extensions.Configuration;

namespace XRest.Identity.App.Queries.ExternalAuthenticate.GetActiveProviderList;

public sealed class GetActiveProviderListQueryHandler : IRequestHandler<GetActiveProviderListQuery, ExternalProviderItem[]>
{
	private readonly IConfiguration _configuration;

	public GetActiveProviderListQueryHandler(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public Task<ExternalProviderItem[]> Handle(GetActiveProviderListQuery request, CancellationToken cancellationToken)
	{
		var externalAuthenticateAppUrl = _configuration.GetValue<string>("ExternalAuthenticate:ExternalAuthenticateAppUrl");

		List<ExternalProviderItem> items = new();
		if (_configuration.GetValue<bool>("ExternalAuthenticate:FacebookEnable"))
		{
			items.Add(new ExternalProviderItem
			{
				Name = "Facebook",
				Url = CombineUrl(externalAuthenticateAppUrl, $"authenticate/facebook?clientType={request.ClientType}")
			});
		}

		if (_configuration.GetValue<bool>("ExternalAuthenticate:GoogleEnable"))
		{
			items.Add(new ExternalProviderItem
			{
				Name = "Google",
				Url = CombineUrl(externalAuthenticateAppUrl, $"authenticate/google?clientType={request.ClientType}")
			});
		}

		return Task.FromResult(items.ToArray());
	}

	private static string CombineUrl(string uri1, string uri2)
	{
		uri1 = uri1.TrimEnd('/');

		return $"{uri1}/{uri2}";
	}


}
