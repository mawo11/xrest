using XRest.Orders.Contracts.Request.Basket;

namespace XRest.Orders.App.Domain;

public class BasketItem
{
	private readonly IBasketContext _basketContext;

	public BasketItem(BasketItemSelectedProduct selectedProducts,
		ReadOnlyProduct product,
		string id,
		IBasketContext basketContext)
	{
		_basketContext = basketContext;
		Id = id;
		SelectedProducts = selectedProducts;
		Product = product;
	}

	/// <summary>
	/// wybrany produkt, łącznie z głównym produktem
	/// </summary>
	public BasketItemSelectedProduct SelectedProducts { get; }

	/// <summary>
	/// Referencyjny wybrany produkt
	/// </summary>
	public ReadOnlyProduct Product { get; }

	public string Id { get; private set; }

	public decimal Total { get; protected set; }

	public void AssignNewId(string id)
	{
		Id = id;
	}

	public virtual decimal CalculateTotalPrice()
	{
		decimal total = CalculateItemPrice();

		if (Product.Package != null)
		{
			total += Calc(Product.Package.Price, _basketContext.DiscountValue);
		}

		Total = total;

		return Total;
	}

	internal BasketItem Clone()
	{
		BasketItemSelectedProduct basketItemSelectedProduct = new()
		{
			Id = SelectedProducts.Id,
			Note = null,
			ProductId = SelectedProducts.ProductId,
			SubProducts = Clone(SelectedProducts.SubProducts!)
		};

		return new BasketItem(basketItemSelectedProduct, Product, Id, _basketContext);
	}

	private static BasketSubProduct[] Clone(BasketSubProduct[] subProducts)
	{
		if (subProducts == null)
		{
			return Array.Empty<BasketSubProduct>();
		}

		return subProducts
			.Select(x => new BasketSubProduct
			{
				Id = x.Id,
				Label = x.Label,
				ProductSets = Clone(x.ProductSets!)
			})
			.ToArray();
	}

	private static BasketSubProductProductSetItem[] Clone(BasketSubProductProductSetItem[] productSets)
	{
		if (productSets == null)
		{
			return Array.Empty<BasketSubProductProductSetItem>();
		}

		return productSets
			.Select(x => new BasketSubProductProductSetItem
			{
				ProductSetId = x.ProductSetId,
				ProductSetItemId = x.ProductSetItemId
			})
			.ToArray();
	}

	private decimal CalculateItemPrice()
	{
		decimal price = 0;

		switch (Product.Type)
		{
			case ProductType.Product:
				price = Calc(Product.GetPriceForRestaurant(_basketContext.RestaurantId), _basketContext.DiscountValue);
				break;
		}

		return price;
	}

	private static decimal Calc(decimal price, decimal discountValue)
	{
		if (discountValue > 0)
		{
			return Math.Round(price - (price * discountValue / 100), 2);
		}

		return price;
	}
}
