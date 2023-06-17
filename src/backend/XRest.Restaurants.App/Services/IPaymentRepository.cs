using XRest.Restaurants.App.Domain;

namespace XRest.Restaurants.App.Services;

public interface IPaymentRepository
{
	ValueTask<Payment[]> GetAllPaymentsAsync();
}
