using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Queries.Customers.GetCustomerPointHistory;

public record GetCustomerPointHistoryQuery(int AccountId) : IRequest<CustomerPointsHistory>;