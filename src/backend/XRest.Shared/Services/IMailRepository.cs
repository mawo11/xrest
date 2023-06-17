using XRest.Shared.Domain;

namespace XRest.Shared.Services;

public interface IMailRepository
{
	ValueTask<bool> SaveMailAsync(Mail mail);
}