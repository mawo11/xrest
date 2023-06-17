using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Queries.Customers.GetMyData;

public record GetMyDataQuery(int AccountId) : IRequest<MyData>;