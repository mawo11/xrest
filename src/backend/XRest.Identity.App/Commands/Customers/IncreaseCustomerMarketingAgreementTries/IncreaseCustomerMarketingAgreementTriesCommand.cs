using MediatR;

namespace XRest.Identity.App.Commands.Customers.IncreaseCustomerMarketingAgreementTries;

public record  IncreaseCustomerMarketingAgreementTriesCommand(int AccountId) : IRequest<bool>;