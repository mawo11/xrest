using XRest.Orders.Contracts.Request.Basket;
using System.Security.Cryptography;
using System.Text;

namespace XRest.Orders.App.Services;

public sealed class FingerprintGenerator : IFingerprintGenerator
{
	public string Generate(string basketKey, BasketItemSelectedProduct basketItemSelectedProduct)
	{
		StringBuilder builder = new();
		builder.Append(basketKey);
		builder.Append(basketItemSelectedProduct.ProductId);

		if (basketItemSelectedProduct.SubProducts != null)
		{
			foreach (var subProduct in basketItemSelectedProduct.SubProducts)
			{
				builder.Append(subProduct.Id);
				if (subProduct.ProductSets != null)
				{
					foreach (var productSet in subProduct.ProductSets)
					{
						builder.Append(productSet.ProductSetId);
						builder.Append(productSet.ProductSetItemId);
					}
				}

				if (!string.IsNullOrEmpty(basketItemSelectedProduct.Note))
				{
					builder.Append(basketItemSelectedProduct.Note);
				}
			}
		}

		return CalculateMD5Hash(builder.ToString());
	}

	private static string CalculateMD5Hash(string input)
	{
		using MD5 md5 = MD5.Create();
		byte[] inputBytes = Encoding.ASCII.GetBytes(input);
		byte[] hash = md5.ComputeHash(inputBytes);
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < hash.Length; i++)
		{
			sb.Append(hash[i].ToString("X2"));
		}

		return sb.ToString();
	}
}
