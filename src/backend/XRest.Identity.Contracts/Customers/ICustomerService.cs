using XRest.Identity.Contracts.Common;
using XRest.Identity.Contracts.Customers.Responses;
using RestEase;

namespace XRest.Identity.Contracts.Customers;

public interface ICustomerService
{
	[Get("/customer/{customerId}/data")]
	Task<ApiOperationResultData<CustomerData>?> GetCustomerData([Path] int customerId);
}
