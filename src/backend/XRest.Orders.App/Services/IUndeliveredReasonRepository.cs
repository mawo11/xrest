using XRest.Orders.App.Domain;

namespace XRest.Orders.App.Services;

public interface IUndeliveredReasonRepository
{
	ValueTask<UndeliveredReason[]> GetUndeliveredReasonsAsync(SourceType? sourceType = null);

	ValueTask<string?> GetReasonById(int id);
}
