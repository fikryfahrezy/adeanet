using System.Text.Json.Serialization;

namespace Adea.Services.UserService;

public record RegisterRequestBodyDTO
{
	[JsonPropertyName("username")]
	public string Username { get; set; } = "";

	[JsonPropertyName("password")]
	public string Password { get; set; } = "";

	[JsonPropertyName("is_officer")]
	public bool IsOfficer { get; set; }
}

public record RegisterResponseBodyDTO
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = "";

	[JsonPropertyName("is_officer")]
	public bool IsOfficer { get; set; }
}

public record LoginRequestBodyDTO
{
	[JsonPropertyName("username")]
	public string Username { get; set; } = "";

	[JsonPropertyName("password")]
	public string Password { get; set; } = "";
}

public record LoginResponseBodyDTO
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = "";

	[JsonPropertyName("is_officer")]
	public bool IsOfficer { get; set; }
}
