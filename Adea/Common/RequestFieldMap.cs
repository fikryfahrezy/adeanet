using Microsoft.AspNetCore.Mvc;

namespace Adea.Common;

public static class RequestFieldMap
{
	public static string GetPropertyName(Type propertyType, string propertyName)
	{
		var jsonPropertyName =
			propertyType.GetProperties()
			.Where(p => p.Name == propertyName)
			.Select(p => p.GetCustomAttributes(typeof(BindPropertyAttribute), true)
							.Cast<BindPropertyAttribute>()
							.FirstOrDefault()
							?.Name
			)
			.FirstOrDefault();

		return jsonPropertyName != null ? jsonPropertyName : propertyName;
	}
}

