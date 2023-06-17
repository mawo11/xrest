
using XRest.Orders.App.Domain;
using XRest.Orders.Infrastructure.MsSql.Internal.Online;

namespace XRest.Orders.App.Services;
internal sealed class OrderViewBuilder : IOrderViewBuilder
{
	private static readonly Dictionary<OrderStatus, string> OrderStatusText = new Dictionary<OrderStatus, string>
	{
		{ OrderStatus.Created, "Utworzono" },
		{ OrderStatus.Accepted, "Zaakceptowane" },
		{ OrderStatus.Cancel, "Anulowane" },
		{ OrderStatus.Reject, "Odrzucone" },
		{ OrderStatus.InDelivery, "W doręczeniu" },
		{ OrderStatus.Delivered, "Dostarczone" },
		{ OrderStatus.NotDelivered, "Nie dostarczony" },
		{ OrderStatus.PaymentWaiting, "W oczekiwaniu na płatność" },
		{ OrderStatus.Considered, "Rozpatrzone" },
		{ OrderStatus.PostPoned, "Schowek" },
		{ OrderStatus.WaitingForRealization, "Oczekiwanie na przetworzenie" },
		{ OrderStatus.Approved, "Zatwierdzono" },
		{ OrderStatus.InRealization, "W realizacji" }
	};

	private readonly IReadOnlyProductRepository _readOnlyProductRepository;

	public OrderViewBuilder(IReadOnlyProductRepository readOnlyProductRepository)
	{
		_readOnlyProductRepository = readOnlyProductRepository;
	}

	public OrderView Build(OnlineOrderTable onlineOrderTable,
		OnlineInvoice onlineInvoice,
		IEnumerable<OnlineOrderPositionTable> products,
		IEnumerable<OnlineOrderHistory> orderHistory,
		IEnumerable<UndeliveredReason> undeliveredReasons)
	{

		var orderView = new OrderView
		{
			AddressCity = onlineOrderTable.City,
			AddressHouseNumber = onlineOrderTable.HouseNumber,
			AddressStreet = onlineOrderTable.Street,
			AddressStreetNumber = onlineOrderTable.StreetNumber,
			Amount = onlineOrderTable.Total,
			CreateDateText = onlineOrderTable.Created.ToString("yyyy-MM-dd HH:mm:ss"),
			Customer = onlineOrderTable.CustomerFistname == null ?
						String.Empty :
						$"{onlineOrderTable.CustomerFistname} {onlineOrderTable.CustomerLastname}",
			Driver = onlineOrderTable.Driver,
			DriverId = onlineOrderTable.DriverId,
			Email = onlineOrderTable.Email,
			Firstname = onlineOrderTable.CustomerFistname,
			Id = onlineOrderTable.Id,
			Lastname = onlineOrderTable.CustomerLastname,
			ModifyDate = onlineOrderTable.ModifyDate,
			ModifyDateText = onlineOrderTable.ModifyDate.ToString("yyyy-MM-dd HH:mm:ss"),
			Note = onlineOrderTable.Note,
			Payment = onlineOrderTable.PaymentName,
			PaymentStatus = onlineOrderTable.PaymentStatus,
			PaymentStatusText = Map(onlineOrderTable.PaymentStatus),
			Phone = onlineOrderTable.Phone,
			Restaurant = onlineOrderTable.Restaurant,
			RestaurantId = onlineOrderTable.RestaurantId,
			Source = Map(onlineOrderTable.Source),
			SourceId = (byte)onlineOrderTable.Source,
			Status = OrderStatusText[onlineOrderTable.Status],
			StatusId = (byte)onlineOrderTable.Status,
			StatusInfo = onlineOrderTable.StatusInfo,
			TermTime = onlineOrderTable.Term.ToString(@"hh\:mm"),
			TermType = Map(onlineOrderTable.TermType),
			TermTypeId = (byte)onlineOrderTable.TermType,
			Worker = onlineOrderTable.Worker
		};

		if (onlineOrderTable.Status == OrderStatus.NotDelivered)
		{
			if (int.TryParse(onlineOrderTable.StatusInfo, out int tempId))
			{
				orderView.StatusInfo = undeliveredReasons.FirstOrDefault(x => x.Id == tempId)?.Reason;
			}
		}

		BuildInvoide(orderView, onlineInvoice);
		BuildOrderHistory(orderView, orderHistory);
		BuildPositions(orderView, onlineOrderTable, products);

		return orderView;
	}

	private static string? Map(TermType termType)
	{
		switch (termType)
		{
			case TermType.Now:
				return "Teraz";
			case TermType.OnHour:
				return "Na podaną godzinę";
		}

		return null;
	}

	private static string? Map(OrderSource source)
	{
		switch (source)
		{
			case OrderSource.CallCenter:
				return "CallCenter";
			case OrderSource.Ordering:
				return "Ordering";
		}

		return null;
	}

	private void BuildPositions(OrderView orderView, OnlineOrderTable onlineOrderTable, IEnumerable<OnlineOrderPositionTable> products)
	{
		var orderPositions = products
					.Where(x => x.TypeId == ProductType.Product &&
					x.FiscalPrint)
					.OrderBy(x => x.Index)
					.Select(x => new OrderViewPosition
					{
						ProductId = x.ProductId,
						Name = x.DisplayName,
						Price = x.Price,
						BasePrice = x.BasePrice,
						Index = x.Index,
						Note = x.Note,
						VatId = x.VatId,
						VatName = x.VatName,
						VatGroup = x.VatGroup,
						OrderProductId = x.OrderProductId,
						FromSource = x.FromSourceId
					})
					.ToList();

		int ordinalNumber = 1;
		List<OrderViewPosition> orderViewPositions = new List<OrderViewPosition>();
		foreach (var orderPosition in orderPositions)
		{
			orderPosition.OrdinalNumber = ordinalNumber++;
			orderViewPositions.Add(orderPosition);

			string[] addons = products
				.Where(x => x.FromSourceId == 2 && x.SubIndex == orderPosition.Index)
				.Select(x => x.DisplayName!)
				.ToArray();
			orderPosition.SubProducts = string.Join(",", addons);
			orderPosition.VatValue = orderPosition.VatValue;

			string? fiscalName = orderPosition.FiscalName;

			if (string.IsNullOrEmpty(fiscalName))
			{
				fiscalName = orderPosition.Name;
			}

			orderPosition.FiscalName = fiscalName;
		}

		if (onlineOrderTable.TransportAmount > 0)
		{
			string transportName = "Dowóz";

			var transportProduct = _readOnlyProductRepository.GetReadOnlyProductById(1);
			string? newName = transportProduct?.GetFiscalName(onlineOrderTable.RestaurantId);

			if (string.IsNullOrEmpty(newName))
			{
				newName = transportProduct?.Name;
			}

			if (!string.IsNullOrEmpty(newName))
			{
				transportName = newName;
			}

			OrderViewPosition transportPosition = new OrderViewPosition();
			transportPosition.Name = transportName;
			transportPosition.FiscalName = transportName;
			transportPosition.Price = onlineOrderTable.TransportAmount;
			transportPosition.BasePrice = onlineOrderTable.TransportAmount;
			transportPosition.Index = ordinalNumber;
			transportPosition.VatId = 1;
			transportPosition.VatGroup = "A";
			transportPosition.VatValue = 23;
			transportPosition.FromSource = 1;
			transportPosition.OrderProductId = -1;
			transportPosition.OrdinalNumber = ordinalNumber++;
			orderViewPositions.Add(transportPosition);
		}

		orderView.Positions = orderViewPositions.ToArray();
	}

	private static string? Map(byte paymentStatus)
	{
		return paymentStatus switch
		{
			0 => "Nie zapłacona",
			1 => "Zapłacona",
			_ => null,
		};
	}

	private static void BuildOrderHistory(OrderView orderView, IEnumerable<OnlineOrderHistory> orderHistory)
	{
		if (orderHistory != null)
		{
			orderView.History = orderHistory
				.Select(x => new OrderHistory
				{
					Created = x.Created,
					Id = x.Id,
					Info = x.Info,
					OrderId = orderView.Id,
					Status = OrderStatusText[x.Status]

				})
				.ToArray();
		}
	}

	private static void BuildInvoide(OrderView orderView, OnlineInvoice onlineInvoice)
	{
		if (onlineInvoice != null)
		{
			orderView.Invoice = true;
			orderView.InvoiceAddress = onlineInvoice.Address;
			orderView.InvoiceName = onlineInvoice.Name;
			orderView.InvoiceNip = onlineInvoice.Nip;
			orderView.InvoiceNumber = onlineInvoice.Number;
		}
	}
}
