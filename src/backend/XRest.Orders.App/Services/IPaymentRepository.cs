using XRest.Orders.App.Domain;

namespace XRest.Orders.App.Services;

public interface IPaymentRepository
{
	ValueTask<bool> SavePaymentHistoryAsync(ChangePaymentHistory[] items);
}
