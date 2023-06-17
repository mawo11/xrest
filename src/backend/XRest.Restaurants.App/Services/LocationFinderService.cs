namespace XRest.Restaurants.App.Services;

public class LocationFinderService : ILocationFinderService
{
	private const int MaxLines = 5;
	public const int LocationNotFound = -1;
	private readonly ReaderWriterLockSlim _cacheLock;
	private City[] _cities;

	public LocationFinderService()
	{
		_cacheLock = new ReaderWriterLockSlim();
		_cities = Array.Empty<City>();
	}

	public string[] FindCities(string cityName)
	{
		if (string.IsNullOrEmpty(cityName))
		{
			return Array.Empty<string>();
		}

		_cacheLock.EnterReadLock();
		try
		{
			string? invariantName = cityName.RemoveDiacritics();
			bool invariantNameExists = !string.IsNullOrEmpty(invariantName);

			List<string> result = new();
			foreach (var city in _cities)
			{
				if (string.IsNullOrEmpty(city.Name) || string.IsNullOrEmpty(city.InvariantName)) continue;

				if (city.Name.Contains(cityName, StringComparison.InvariantCultureIgnoreCase) ||
					(invariantNameExists && city.InvariantName.Contains(invariantName!, StringComparison.InvariantCultureIgnoreCase)))
				{
					result.Add(city.Name);
				}

				if (result.Count >= MaxLines)
				{
					break;
				}
			}

			return result.ToArray();
		}
		finally
		{
			_cacheLock.ExitReadLock();
		}
	}

	public int FindLocation(string cityName, string streetName)
	{
		_cacheLock.EnterReadLock();
		try
		{
			City? currentCity = FindCity(cityName);

			if (currentCity == null)
			{
				return LocationNotFound;
			}

			string? invariantStreet = streetName.RemoveDiacritics();

			if (currentCity.Addresses != null)
			{
				foreach (var address in currentCity.Addresses)
				{
					if (string.Compare(address.Street, streetName, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						return address.Id;
					}
				}
			}
		}
		finally
		{
			_cacheLock.ExitReadLock();
		}

		return LocationNotFound;
	}

	public string[] FindStreets(string cityName, string streetName)
	{
		_cacheLock.EnterReadLock();
		try
		{
			List<string> result = new();
			City? currentCity = FindCity(cityName);

			if (currentCity == null)
			{
				return Array.Empty<string>();
			}

			string? invariantStreet = streetName.RemoveDiacritics();
			bool invariantStreetExists = !string.IsNullOrEmpty(invariantStreet);

			if (currentCity.Addresses != null)
			{
				foreach (var address in currentCity.Addresses)
				{
					if (string.IsNullOrEmpty(address.Street) || string.IsNullOrEmpty(address.InvariantStreet)) continue;

					if (address.Street.Contains(streetName, StringComparison.InvariantCultureIgnoreCase) ||
						(invariantStreetExists && address.InvariantStreet.Contains(invariantStreet!, StringComparison.InvariantCultureIgnoreCase)))
					{
						if (result.Find(x => string.Compare(x, address.Street, true) == 0) == null)
						{
							result.Add(address.Street);
						}
					}

					if (result.Count >= MaxLines)
					{
						break;
					}
				}
			}

			return result.ToArray();
		}
		finally
		{
			_cacheLock.ExitReadLock();
		}
	}

	private City? FindCity(string cityName)
	{
		foreach (var city in _cities)
		{
			if (string.Compare(city.Name, cityName, StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				return city;
			}
		}

		return null;
	}

	public void LoadData(IEnumerable<Domain.Address> addresses)
	{
		_cacheLock.EnterWriteLock();
		try
		{
			var citiesSource = addresses
				.GroupBy(x => x.City)
				.ToList();

			_cities =
					citiesSource
					.Where(x => !string.IsNullOrEmpty(x.Key))
					.Select(x => new City
					{
						Name = x.Key,
						InvariantName = x.Key.RemoveDiacritics(),
						Addresses = x
								.Select(y => new Address
								{
									Id = y.Id,
									Street = y.Street,
									InvariantStreet = y.Street.RemoveDiacritics()
								})

								.ToArray()
					})
					.OrderBy(x => x.Name)
					.ToArray();
		}
		finally
		{
			_cacheLock.ExitWriteLock();
		}
	}

	private class City
	{
		public string? Name { get; set; }

		public string? InvariantName { get; set; }

		public Address[]? Addresses { get; set; }
	}

	private class Address
	{
		public int Id { get; set; }

		public string? Street { get; set; }

		public string? InvariantStreet { get; set; }
	}
}
