using XRest.Orders.Contracts.Request.Basket;

namespace XRest.Orders.App.Domain;

public class BasketItemGratis : BasketItem
{
	public BasketItemGratis(BasketItemSelectedProduct selectedProducts,
		ReadOnlyProduct product,
		string id,
		IBasketContext basketContext) : base(selectedProducts, product, id, basketContext)
	{
	}

	public override decimal CalculateTotalPrice()
	{
		const decimal defeualtPrice = (decimal)0.01;
		decimal total = defeualtPrice;
		Total = defeualtPrice;
		if (Product.Package != null)
		{
			Product.Package.Price = defeualtPrice;
			total += defeualtPrice;
		}

		return total;
	}
}
