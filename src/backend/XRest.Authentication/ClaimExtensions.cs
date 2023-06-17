using System.Security.Claims;

namespace XRest.Authentication;

public static class ClaimExtensions
{
	public static int AsInt(this Claim? claim, int def = default)
	{
		if (claim == null || string.IsNullOrEmpty(claim.Value))
		{
			return def;
		}

		return int.TryParse(claim.Value, out int val) ? val : def;
	}

	public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
	{
		return claimsPrincipal.FindFirst(CustomClaims.UserId).AsInt();
	}

	public static int GetRestaurantId(this ClaimsPrincipal claimsPrincipal)
	{
		var val = claimsPrincipal.FindFirst(CustomClaims.RestaurantId)?.Value;

		if (int.TryParse(val, out int restaurantId))
		{
			return restaurantId;
		}

		return 0;
	}
}