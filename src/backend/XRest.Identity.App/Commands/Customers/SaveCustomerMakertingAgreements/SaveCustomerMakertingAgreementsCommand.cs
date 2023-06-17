using XRest.Identity.Contracts.Common;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Commands.Customers.SaveCustomerMakertingAgreements;

public record SaveCustomerMakertingAgreementsCommand(IEnumerable<CustomerMarketingAgreement> Items, int AccountId) : IRequest<ApiOperationResult>;
