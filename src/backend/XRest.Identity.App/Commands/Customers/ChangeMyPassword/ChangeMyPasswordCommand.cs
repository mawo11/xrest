using XRest.Identity.Contracts.Common;
using MediatR;

namespace XRest.Identity.App.Commands.Customers.ChangeMyPassword;

public record ChangeMyPasswordCommand(int AccountId, string? Password) : IRequest<ApiOperationResult>;
