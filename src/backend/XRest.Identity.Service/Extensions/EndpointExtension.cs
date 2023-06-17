using System.Reflection;

namespace XRest.Identity.Service.Extensions;

public static class EndpointExtension
{
	public static void RegisterApiEndpoints(this WebApplication app, Assembly source)
	{
		var endpointsType = source
					.GetTypes()
					.Where(x => x.Name.EndsWith("Endpoint", System.StringComparison.InvariantCultureIgnoreCase))
					.ToArray();


		object[] parameters = new[] { app };

		foreach (var endpoint in endpointsType)
		{
			var registerMethod = endpoint.GetMethod("Register", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
			if (registerMethod != null)
			{
				registerMethod.Invoke(null, parameters);
			}
		}

	}
}
