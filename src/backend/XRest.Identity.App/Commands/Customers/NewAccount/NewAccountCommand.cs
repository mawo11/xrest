using XRest.Identity.Contracts.Customers.Responses;
using MediatR;
using XRest.Identity.Contracts.Customers.Request;

namespace XRest.Identity.App.Commands.Customers.NewAccount;

public record NewAccountCommand(NewAccountRequest NewAccount) : IRequest<NewAccountResponse>;