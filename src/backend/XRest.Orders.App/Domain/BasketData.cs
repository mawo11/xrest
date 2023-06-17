namespace XRest.Orders.App.Domain;

public class BasketData : IBasketContext
{
	private readonly object _locker = new();
	private bool _locked;

	public string BasketKey { get; set; } = string.Empty;

	public DateTime LastActiviy { get; set; }

	public int RestaurantId { get; set; }

	public List<BasketItem> Items { get; set; } = new List<BasketItem>();

	public int TransportZoneId { get; set; }

	public DeliveryType Type { get; set; }

	public OrderSource Source { get; set; }

	public decimal DeliveryPrice { get; set; }

	public decimal Total { get; set; }

	public decimal TotalWithoutDelivery { get; set; }

	public bool IsLocked => _locked;

	public ReadOnlyProduct? BagProduct { get; set; }

	public decimal DiscountValue { get; set; }

	public DiscountCodeToBurn? DiscountCode { get; set; }

	public BasketItemGratis? GratisProduct { get; set; }

	public bool TryLock()
	{
		lock (_locker)
		{
			if (!_locked)
			{
				_locked = true;
				return true;
			}
		}

		return false;
	}

	public void Unlock()
	{
		lock (_locker)
		{
			_locked = false;
		}
	}
}
