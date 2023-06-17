using XRest.Identity.Contracts.Common;
using MediatR;

namespace XRest.Identity.App.Commands.Customers.DisableAccount;

public record DisableAccountCommand(int AccountId) : IRequest<ApiOperationResult>;