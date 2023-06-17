
using XRest.Orders.App.Domain;
using XRest.Orders.Contracts.Request.Basket;
using System.Data;

namespace XRest.Orders.Infrastructure.MsSql.Internal.Online;
internal class OnlineOrderSerializator : IOnlineOrderSerializator
{
	public (DataTable header, DataTable rows) Serialize(NewOnlineOrder newOnlineOrder)
	{
		return (CreateHeader(newOnlineOrder), CreateRows(newOnlineOrder));
	}

	private static DataTable CreateHeader(NewOnlineOrder newOnlineOrder)
	{
		DataTable table = new();
		table.Columns.Add("customer_id");
		table.Columns.Add("restaurant_id", typeof(int));
		table.Columns.Add("create_date", typeof(DateTime));
		table.Columns.Add("address_city");
		table.Columns.Add("address_street");
		table.Columns.Add("address_street_number");
		table.Columns.Add("address_house_number");
		table.Columns.Add("worker_id");
		table.Columns.Add("amount", typeof(decimal));
		table.Columns.Add("transport_amount", typeof(decimal));
		table.Columns.Add("email", typeof(string));
		table.Columns.Add("phone", typeof(string));
		table.Columns.Add("note", typeof(string));
		table.Columns.Add("status_id", typeof(byte));
		table.Columns.Add("payment_type_id", typeof(byte));
		table.Columns.Add("term_type_id", typeof(byte));
		table.Columns.Add("term", typeof(TimeSpan));
		table.Columns.Add("type_of_delivery_id", typeof(byte));
		table.Columns.Add("modify_date", typeof(DateTime));
		table.Columns.Add("restaurant_transport_id");
		table.Columns.Add("terms_of_use", typeof(bool));
		table.Columns.Add("realization", typeof(bool));
		table.Columns.Add("marketing", typeof(bool));
		table.Columns.Add("audit_date", typeof(DateTime));
		table.Columns.Add("order_day", typeof(int));
		table.Columns.Add("firstname", typeof(string));
		table.Columns.Add("discount", typeof(int));
		table.Columns.Add("source", typeof(byte));

		table.Rows.Add(
			newOnlineOrder.CustomerId,
			newOnlineOrder.RestaurantId,
			newOnlineOrder.Created,
			newOnlineOrder.OrderAddress?.City ?? string.Empty,
			newOnlineOrder.OrderAddress?.Street ?? string.Empty,
			newOnlineOrder.OrderAddress?.StreetNumber ?? string.Empty,
			newOnlineOrder.OrderAddress?.HouseNumber ?? string.Empty,
			newOnlineOrder.WorkedId,
			newOnlineOrder.Amount,
			newOnlineOrder.TransportAmount,
			newOnlineOrder.Email,
			newOnlineOrder.Phone,
			newOnlineOrder.Note,
			newOnlineOrder.Status,
			newOnlineOrder.PaymentId,
			newOnlineOrder.TermType,
			newOnlineOrder.TermHour,
			newOnlineOrder.DeliveryType,
			newOnlineOrder.Modified,
			newOnlineOrder.TransportZoneId == -1 ? null : newOnlineOrder.TransportZoneId,
			newOnlineOrder.TermOfUse,
			newOnlineOrder.Realization,
			newOnlineOrder.Marketing,
			newOnlineOrder.AuditDate,
			newOnlineOrder.OrderDay,
			newOnlineOrder.Firstname,
			newOnlineOrder.Discount == 0 ? null : newOnlineOrder.Discount,
			newOnlineOrder.Source);

		return table;
	}

	private static DataTable CreateRows(NewOnlineOrder newOnlineOrder)
	{
		DataTable table = new();
		table.Columns.Add("product_id", typeof(int));
		table.Columns.Add("price", typeof(decimal));
		table.Columns.Add("points", typeof(short));
		table.Columns.Add("type_id", typeof(byte));
		table.Columns.Add("order_product_id", typeof(int));
		table.Columns.Add("display_name", typeof(string));
		table.Columns.Add("from_source_id", typeof(byte));
		table.Columns.Add("index", typeof(short));
		table.Columns.Add("note", typeof(string));
		table.Columns.Add("sub_index", typeof(short));
		table.Columns.Add("base_price", typeof(decimal));
		table.Columns.Add("vat");

		Context ctx = new(newOnlineOrder.DeliveryType, newOnlineOrder.RestaurantId, table);

		if (newOnlineOrder.Items != null)
		{
			foreach (var item in newOnlineOrder.Items)
			{
				switch (item.Product.Type)
				{
					case ProductType.Product:
						if (item.SelectedProducts != null && item.SelectedProducts.SubProducts != null)
						{
							var subProduct = item.SelectedProducts.SubProducts.FirstOrDefault(x => x.Id == item.Product.Id);
							AddSingleProduct(ctx, item.Product, subProduct, item.SelectedProducts.Note, null, newOnlineOrder.Discount);
						}
						break;
				}

				ctx.OrderProductId++;
			}
		}

		if (newOnlineOrder.GratisProduct != null &&
			newOnlineOrder.GratisProduct.SelectedProducts != null &&
			newOnlineOrder.GratisProduct.SelectedProducts.SubProducts != null)
		{
			var subProduct = newOnlineOrder.GratisProduct.SelectedProducts.SubProducts.FirstOrDefault(x => x.Id == newOnlineOrder.GratisProduct.Product.Id);
			AddSingleProduct(ctx,
				newOnlineOrder.GratisProduct.Product,
				subProduct,
				newOnlineOrder.GratisProduct.SelectedProducts.Note,
				(decimal)0.01,
				0,
				ProductSource.ExtraProduct);
		}

		if (newOnlineOrder.BagProduct != null)
		{
			AddSingleProduct(ctx, newOnlineOrder.BagProduct, null, null);
		}

		return table;
	}

	private static void AddSingleProduct(Context ctx,
		ReadOnlyProduct product,
		BasketSubProduct? subProduct,
		string? note,
		decimal? price = null,
		decimal discount = 0,
		ProductSource productSource = ProductSource.MainProduct)
	{
		short productIndex = ctx.Index++;

		ctx.Table.Rows.Add(
		  product.Id,
		  Calc(price ?? product.GetPriceForRestaurant(ctx.RestaurantId), discount),
		  product.Points,
		  product.Type,
		  ctx.OrderProductId,
		  product.GetFiscalName(ctx.RestaurantId),
		  productSource,
		  productIndex,
		  note,
		  null,
		  price ?? product.GetPriceForRestaurant(ctx.RestaurantId),
		  product.VatValue);

		if (subProduct != null && product.ProductSets != null && product.ProductSets.Length > 0)
		{
			foreach (var productSet in product.ProductSets)
			{
				foreach (var productSetItem in productSet.Items!)
				{
					if (subProduct.ProductSets == null) continue;

					if (subProduct.ProductSets.Any(x => x.ProductSetId == productSet.Id && x.ProductSetItemId == productSetItem.Id))
					{
						ctx.Table.Rows.Add(
						   productSetItem.ProductId,
						   0,
						   0,
						   ProductType.Product,
						   ctx.OrderProductId,
						   productSetItem.Product!.DisplayName,
						   ProductSource.ProductSet,
						   ctx.Index++,
						   null,
						   productIndex,
						   0,
						   productSetItem.Product.VatValue);
					}
				}
			}
		}

		if (product.Package != null)
		{
			ctx.Table.Rows.Add(
				product.Package.Id,
				Calc(product.Package.Price, discount),
				0,
				ProductType.Product,
				ctx.OrderProductId,
				product.Package.DisplayName,
				ProductSource.Bundle,
				ctx.Index++,
				null,
				null,
				product.Package.Price,
				product.Package.VatValue);
		}
	}

	private static decimal Calc(decimal price, decimal discountValue)
	{
		if (discountValue > 0)
		{
			return Math.Round(price - price * discountValue / 100, 2);
		}

		return price;
	}

	private class Context
	{
		public Context(App.Domain.DeliveryType deliveryType, int restaurantId, DataTable table)
		{
			DeliveryType = deliveryType;
			RestaurantId = restaurantId;
			Table = table;
		}

		public int RestaurantId { get; set; }

		public App.Domain.DeliveryType DeliveryType { get; set; }

		public DataTable Table { get; set; }

		internal short Index { get; set; }

		internal int OrderProductId { get; set; }
	}
}
