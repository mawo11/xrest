using XRest.Orders.App.Domain;
using XRest.Shared.Extensions;

namespace XRest.Orders.Infrastructure.MsSql.Internal.Online;

internal class OnlineOrderBuilder : IOnlineOrderBuilder
{
	public OnlineOrder Build(OnlineOrderTable onlineOrderTable, OnlineInvoice onlineInvoice, IEnumerable<OnlineOrderPositionTable> products)
	{
		if (onlineOrderTable == null)
		{
			return OnlineOrder.NotFound;
		}

		OnlineOrder onlineOrder = new()
		{
			Created = onlineOrderTable.Created,
			DeliveryType = onlineOrderTable.DeliveryType,
			Discount = onlineOrderTable.Discount,
			Driver = onlineOrderTable.Driver,
			DriverId = onlineOrderTable.DriverId,
			Email = onlineOrderTable.Email.Cleanup(),
			Id = onlineOrderTable.Id,
			Note = onlineOrderTable.Note?.Cleanup(),
			OrderDay = onlineOrderTable.OrderDay,
			PayedOnline = onlineOrderTable.PayedOnline,
			PaymentId = onlineOrderTable.PaymentId,
			Phone = onlineOrderTable.Phone,
			RestaurantId = onlineOrderTable.RestaurantId,
			Source = onlineOrderTable.Source,
			Status = onlineOrderTable.Status,
			Term = onlineOrderTable.Term != TimeSpan.Zero ? onlineOrderTable.Term.ToString("hh:mm") : string.Empty,
			TermType = onlineOrderTable.TermType,
			Total = onlineOrderTable.Total,
			OrderAddress = new OnlineOrderAddress
			{
				City = onlineOrderTable.City.Cleanup(),
				HouseNumber = onlineOrderTable.HouseNumber.Cleanup(),
				Street = onlineOrderTable.Street.Cleanup(),
				StreetNumber = onlineOrderTable.StreetNumber.Cleanup()
			},
			OnlineInvoice = onlineInvoice
		};

		var productGroups = products.GroupBy(x => x.OrderProductId)
			.ToList();

		List<OnlineOrderItem> orderItems = new();

		foreach (var orderProduct in productGroups)
		{
			var items = orderProduct
				.OrderBy(x => x.Index)
				.ToList();

			var firstproduct = items.FirstOrDefault();
			if (firstproduct != null && firstproduct.TypeId == ProductType.Product)
			{
				var orderItem = AddSingleProduct(items);
				if (orderItem != null)
				{
					orderItems.Add(orderItem);
				}

				continue;
			}
		}

		if (onlineOrderTable.TransportAmount > 0)
		{
			orderItems.Add(new OnlineOrderItem
			{
				Product = new OnlineProductProduct
				{
					Name = "Dowóz",
					Price = onlineOrderTable.TransportAmount
				},
				Total = onlineOrderTable.TransportAmount
			});
		}

		onlineOrder.Products = orderItems.ToArray();
		return onlineOrder;
	}

	private static OnlineOrderItem? AddSingleProduct(List<OnlineOrderPositionTable> items)
	{
		OnlineOrderPositionTable? mainProduct = items.FirstOrDefault(x => x.FromSourceId == 1 || x.FromSourceId == 4);
		if (mainProduct != null)
		{
			OnlineOrderItem item = new()
			{
				Total = mainProduct.Price,
				Product = new OnlineProductProduct
				{
					Id = mainProduct.ProductId,
					Name = mainProduct.DisplayName,
					ProductType = ProductType.Product,
					Price = mainProduct.Price,
					SubProducts = string.Join(", ", items
							.Where(x => x.SubIndex == mainProduct.Index)
							.Select(static x => x.DisplayName)
							.ToArray()),
					ProductGroupId = mainProduct.ProductGroupId
				}
			};

			var package = items.FirstOrDefault(x => x.FromSourceId == 3);
			if (package != null)
			{
				item.Product.Package = new OnlineProductPackage
				{
					Id = package.ProductId,
					Name = package.DisplayName,
					Price = package.Price
				};
			}

			return item;
		}

		return null;
	}
}
