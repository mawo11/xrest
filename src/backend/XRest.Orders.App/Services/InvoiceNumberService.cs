using XRest.Orders.App.Domain;
using XRest.Shared.Services;

namespace XRest.Orders.App.Services;

public class InvoiceNumberService : IInvoiceNumberService
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IOnlineOrderInvoiceRepository _onlineOrderInvoiceRepository;

	public InvoiceNumberService(IDateTimeProvider dateTimeProvider, IOnlineOrderInvoiceRepository onlineOrderInvoiceRepository)
	{
		_dateTimeProvider = dateTimeProvider;
		_onlineOrderInvoiceRepository = onlineOrderInvoiceRepository;
	}

	public async ValueTask<string?> CreateAsync(int restaurantId)
	{
		var now = _dateTimeProvider.Now;

		var currentValue = await _onlineOrderInvoiceRepository.GetNextInvoiceNumberAsync(restaurantId, now.Year, now.Month);

		if (currentValue != NewInvoiceNumberResult.Invalid)
		{
			return string.Format("FA{0}{1}{2}{3}",
				now.Year,
				now.Month.ToString("D2"),
				currentValue.Number.ToString("D4"),
				restaurantId.ToString("D2"));
		}

		return null;
	}
}