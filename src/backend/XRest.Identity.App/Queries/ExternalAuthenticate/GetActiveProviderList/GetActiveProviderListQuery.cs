using XRest.Identity.Contracts.ExternalAuthenticate.Responses;
using MediatR;

namespace XRest.Identity.App.Queries.ExternalAuthenticate.GetActiveProviderList;

public record GetActiveProviderListQuery(int ClientType) : IRequest<ExternalProviderItem[]>;