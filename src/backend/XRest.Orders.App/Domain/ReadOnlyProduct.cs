using System.Xml;

namespace XRest.Orders.App.Domain;

public class ReadOnlyProduct
{
	public int Id { get; set; }

	public string? DisplayName { get; set; }

	public string? DisplayNameTranslations { get; set; }

	public string Name { get; set; } = string.Empty;

	public string? Plu { get; set; }

	public decimal Price { get; set; }

	public bool Visible { get; set; }

	public int ProductGroupId { get; set; }

	public ReadOnlyProductGroup? ProductGroup { get; set; }

	public string? Description { get; set; }

	public string? DescriptionTranslations { get; set; }

	public bool Archive { get; set; }

	public short Points { get; set; }

	public ProductType Type { get; set; }

	public bool FiscalPrint { get; set; }

	public int? PackageId { get; set; }

	public bool IsPackage { get; set; }

	public short TakePoints { get; set; }

	public ProductDestination Destination { get; set; }

	public string? BackgroundColor { get; set; }

	public string? TextColor { get; set; }

	public byte Printer { get; set; }

	public ReadOnlyProductBundle[]? Bundles { get; set; }

	public int VatId { get; set; }

	public string? VatName { get; set; }

	public string? VatGroup { get; set; }

	public int VatValue { get; set; }

	public ReadOnlyProduct? Package { get; set; }

	public DateTime? ImageAudit { get; set; }

	public ReadOnlyProductSet[]? ProductSets { get; set; }

	public List<ReadOnlyRestaurantContext> RestaurantSpecificData { get; set; } = new List<ReadOnlyRestaurantContext>();

	public decimal GetPriceForRestaurant(int restaurantId)
	{
		var context = RestaurantSpecificData.Find(x => x.RestaurantId == restaurantId);
		decimal priceForRestaurant = context?.Data?.Price ?? 0;

		return priceForRestaurant == 0 ? Price : priceForRestaurant;
	}

	public string? GetFiscalName(int restaurantId)
	{
		var context = RestaurantSpecificData.Find(x => x.RestaurantId == restaurantId);
		if (string.IsNullOrEmpty(context?.Data?.FiscalName))
		{
			return DisplayName;
		}

		return context?.Data?.FiscalName ?? string.Empty;
	}

	public bool IsAllowedOnlineForRestaurant(int restaurantId)
	{
		if (!Archive && Visible && (Destination == ProductDestination.All || Destination == ProductDestination.Online))
		{
			var context = RestaurantSpecificData.Find(x => x.RestaurantId == restaurantId);
			if (context != null)
			{
				if (context.Data == null)
				{
					return false;
				}

				DateTime now = DateTime.Now;
				byte dayOfWeek = (byte)now.DayOfWeek;
				TimeSpan time = new(now.Hour, now.Minute, 0);

				return !(context?.Data?.Hidden ?? false);
			}
		}

		return false;
	}

	public string? GetTranslatedDisplayName(string? lang)
	{
		lang ??= "pl";

		if (string.IsNullOrEmpty(DisplayNameTranslations))
		{
			return DisplayName;
		}

		try
		{
			XmlDocument xmlDocument = new();
			xmlDocument.LoadXml(DisplayNameTranslations);

			if (xmlDocument.DocumentElement == null)
			{
				return DisplayName;
			}

			var nodes = xmlDocument.DocumentElement.SelectSingleNode("/langs/" + lang);

			return nodes?.InnerText ?? DisplayName;
		}
		catch
		{
			return DisplayName;
		}
	}
}
