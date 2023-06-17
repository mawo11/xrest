using XRest.Identity.Contracts.Common;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Commands.Customers.SaveDetails;

public record SaveDetailsCommand(int AccountId, MyData MyData) : IRequest<ApiOperationResult>;
