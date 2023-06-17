using System.Security.Cryptography;
using System.Text;

namespace XRest.Identity.App.Services;

public sealed class PasswordHasher : IPasswordHasher, IDisposable
{
	private readonly SHA512 _md5;

	public PasswordHasher()
	{
		_md5 = SHA512.Create();
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		try
		{
			_md5.Dispose();
		}
		catch { }
	}

	public string HashPassword(string password)
	{
		byte[] inputBytes = Encoding.ASCII.GetBytes($"xrest{password}mawo11");
		byte[] hash = _md5.ComputeHash(inputBytes);
		StringBuilder sb = new();
		for (int i = 0; i < hash.Length; i++)
		{
			sb.Append(hash[i].ToString("X2"));
		}

		return sb.ToString();
	}
}
