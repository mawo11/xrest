namespace XRest.Identity.App.Services;

public interface IPasswordHasher
{
	string HashPassword(string password);
}