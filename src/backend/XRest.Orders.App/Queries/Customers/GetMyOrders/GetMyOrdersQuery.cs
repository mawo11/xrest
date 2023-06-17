using XRest.Orders.Contracts.Responses.Customers;
using MediatR;

namespace XRest.Orders.App.Queries.Customers.GetMyOrders;

public record GetMyOrdersQuery(int AccountId) : IRequest<MyOrdersResponse>;