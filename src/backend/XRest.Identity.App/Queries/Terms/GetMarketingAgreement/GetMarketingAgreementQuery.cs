using XRest.Identity.Contracts.Responses;
using MediatR;

namespace XRest.Identity.App.Queries.Terms.GetMarketingAgreement;

public record GetMarketingAgreementQuery(string? Lang) : IRequest<MarketingAgreement[]>;