namespace XRest.Shared.Domain;

public class Mail
{
	private readonly List<MailReplacamentField> _fields = new();

	public int Id { get; set; }

	public MailStatus Status { get; set; }

	public MailTemplate Template { get; set; }

	public byte Tries { get; set; }

	public string? Errors { get; set; }

	public string? Address { get; set; }

	public string? Subject { get; set; } 

	public IList<MailReplacamentField> Replacements => _fields;

	public void AddReplacement(string key, string value) => Replacements.Add(new MailReplacamentField(key, value));
}
