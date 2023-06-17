namespace XRest.Orders.App.Domain;

public interface IBasketContext
{
	DiscountCodeToBurn? DiscountCode { get; set; }

	decimal DiscountValue { get; set; }

	List<BasketItem> Items { get; set; }

	int RestaurantId { get; set; }

	DeliveryType Type { get; set; }
}