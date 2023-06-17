using System.Text;

namespace XRest.Restaurants.App.Services;

public static class StringExtensions
{
	static readonly Dictionary<char, char> StringToReplace = new()
	{
		{ 'ą', 'a' },
		{ 'ć', 'c' },
		{ 'ę', 'e' },
		{ 'ś', 's' },
		{ 'ń', 'n' },
		{ 'ł', 'l' },
		{ 'ż', 'z' },
		{ 'ź', 'z' },
		{ 'ó', 'o' },
		{ 'Ą', 'A' },
		{ 'Ć', 'C' },
		{ 'Ę', 'E' },
		{ 'Ś', 'S' },
		{ 'Ń', 'N' },
		{ 'Ł', 'L' },
		{ 'Ż', 'Z' },
		{ 'Ź', 'Z' },
		{ 'Ó', 'O' },
	};

	public static string? RemoveDiacritics(this string? s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return null;
		}

		StringBuilder sb = new(s.Length);

		foreach (char c in s)
		{
			if (StringToReplace.ContainsKey(c))
			{
				sb.Append(StringToReplace[c]);
			}
			else
			{
				sb.Append(c);
			}
		}

		return sb.ToString();
	}

	public static int ConvertToInt(this string? input)
	{
		if (int.TryParse(input, out int temp))
		{
			return temp;
		}

		return 0;
	}
}
