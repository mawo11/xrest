using XRest.Identity.Contracts.Common;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Queries.Customers.GetCustomerData;

public record GetCustomerDataQuery(int CustomerId) : IRequest<ApiOperationResultData<CustomerData>>;