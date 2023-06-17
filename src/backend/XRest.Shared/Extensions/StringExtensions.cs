using System.Text;
using System.Xml;

namespace XRest.Shared.Extensions;

public static class StringExtensions
{
	private readonly static char[] ToReplace = new[] { (char)96, (char)39, (char)60, (char)62, (char)47, (char)63, '#', '#', '$', '%', '^', '&', '*', '(', ')', '{', '}', '[', ']', '\\', '|', ';', };

	public static string? Cleanup(this string? text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}

		StringBuilder builder = new StringBuilder(text.Length);

		foreach (char charForTest in text)
		{
			foreach (var toReplace in ToReplace)
			{
				if (charForTest == toReplace)
				{
					continue;
				}

				builder.Append(charForTest);
			}
		}

		return builder.ToString();
	}

	public static string? GetTranslate(this string text, string? lang, string? defaultText)
	{
		if (string.IsNullOrEmpty(text))
		{
			return defaultText;
		}

		if (lang == null)
		{
			lang = "pl";
		}

		try
		{
			XmlDocument xmlDocument = new();
			xmlDocument.LoadXml(text);

			if (xmlDocument.DocumentElement == null)
			{
				return defaultText;
			}

			var nodes = xmlDocument.DocumentElement.SelectSingleNode("/langs/" + lang);

			return nodes?.InnerText ?? defaultText;
		}
		catch
		{
			return defaultText;
		}

	}
}


