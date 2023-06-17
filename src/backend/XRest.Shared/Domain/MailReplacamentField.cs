namespace XRest.Shared.Domain;

public class MailReplacamentField
{
	public MailReplacamentField(string key, string value)
	{
		Key = key;
		Value = value;
	}

	public string Key { get; }

	public string Value { get; }
}
