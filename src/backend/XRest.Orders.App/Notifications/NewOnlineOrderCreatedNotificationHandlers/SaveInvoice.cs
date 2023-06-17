using XRest.Orders.App.Domain;
using XRest.Orders.App.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using XRest.Shared.Services;

namespace XRest.Orders.App.Notifications.NewOnlineOrderCreatedNotificationHandlers;

public class SaveInvoice : INotificationHandler<NewOnlineOrderCreatedNotification>
{
	private readonly ILogger<SaveInvoice> _logger;
	private readonly IInvoiceNumberService _invoiceNumberService;
	private readonly IOnlineOrderInvoiceRepository _onlineOrderInvoiceRepository;
	private readonly IDateTimeProvider _dateTimeProvider;

	public SaveInvoice(ILogger<SaveInvoice> logger,
		IInvoiceNumberService invoiceNumberService,
		IDateTimeProvider dateTimeProvider,
		IOnlineOrderInvoiceRepository onlineOrderInvoiceRepository)
	{
		_dateTimeProvider = dateTimeProvider;
		_logger = logger;
		_invoiceNumberService = invoiceNumberService;
		_onlineOrderInvoiceRepository = onlineOrderInvoiceRepository;
	}

	public async Task Handle(NewOnlineOrderCreatedNotification notification, CancellationToken cancellationToken)
	{
		int orderId = notification.OrderId;
		var newOnlineOrder = notification.NewOnlineOrder;

		if (orderId != -1 && newOnlineOrder.OnlineInvoice != null)
		{
			newOnlineOrder.OnlineInvoice.Number = await _invoiceNumberService.CreateAsync(newOnlineOrder.RestaurantId);
			newOnlineOrder.OnlineInvoice.OrderId = orderId;
			newOnlineOrder.OnlineInvoice.CreateDate = _dateTimeProvider.Now;

			if (string.IsNullOrEmpty(newOnlineOrder.OnlineInvoice.Number) || !await _onlineOrderInvoiceRepository.SaveInvoiceAsync(newOnlineOrder.OnlineInvoice))
			{
				_logger.LogCritical("Nie udało się wygenerować faktury dla zestawu danych: {data}", System.Text.Json.JsonSerializer.Serialize(notification));
			}
		}
	}
}
