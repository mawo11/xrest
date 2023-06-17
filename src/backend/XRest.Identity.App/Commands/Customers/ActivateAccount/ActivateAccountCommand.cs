using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Commands.Customers.ActivateAccount;

public record ActivateAccountCommand(string Token) : IRequest<ActivateAccountResponse>;