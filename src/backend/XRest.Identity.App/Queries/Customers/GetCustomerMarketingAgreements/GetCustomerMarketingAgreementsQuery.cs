using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Queries.Customers.GetCustomerMarketingAgreements;

public record GetCustomerMarketingAgreementsQuery(int AccountId, string? Lang) : IRequest<IEnumerable<CustomerMarketingAgreement>>;
