using System.Text.Json.Serialization;

namespace Adea.Services.Common;

public static class JsonPropertyUtil
{
	public static string GetJsonPropertyName(Type propertyType, string propertyName)
	{
		var jsonPropertyName =
			propertyType.GetProperties()
			.Where(p => p.Name == propertyName)
			.Select(p => p.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true)
							.Cast<JsonPropertyNameAttribute>()
							.FirstOrDefault()
							?.Name)
			.FirstOrDefault();

		return jsonPropertyName != null ? jsonPropertyName : propertyName;
	}
}

