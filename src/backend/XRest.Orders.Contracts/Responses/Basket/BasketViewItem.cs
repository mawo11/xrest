namespace XRest.Orders.Contracts.Responses.Basket
{
	public class BasketViewItem
	{
		public string? Id { get; set; }

		public string? Title { get; set; }

		public string? Note { get; set; }

		public string[]? SubProducts { get; set; }

		public int Count { get; set; }

		public BasketViewItemBundleItem[]? Bundles { get; set; }

		public string? Price { get; set; }

		public bool ReadOnly { get; set; }
	}
}
