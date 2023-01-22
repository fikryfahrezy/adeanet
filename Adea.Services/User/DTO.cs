using System.Text.Json.Serialization;

namespace Adea.Services.User;

public record RegisterRequestBodyDTO
{
	[JsonPropertyNameAttribute("username")]
	public string Username { get; set; } = "";

	[JsonPropertyNameAttribute("password")]
	public string Password { get; set; } = "";

	[JsonPropertyNameAttribute("is_officer")]
	public bool IsOfficer { get; set; }
}

public record RegisterResponseBodyDTO
{
	[JsonPropertyNameAttribute("id")]
	public string Id { get; set; } = "";

	[JsonPropertyNameAttribute("is_officer")]
	public bool IsOfficer { get; set; }
}

public record LoginRequestBodyDTO
{
	[JsonPropertyNameAttribute("username")]
	public string Username { get; set; } = "";

	[JsonPropertyNameAttribute("password")]
	public string Password { get; set; } = "";
}

public record LoginResponseBodyDTO
{
	[JsonPropertyNameAttribute("id")]
	public string Id { get; set; } = "";

	[JsonPropertyNameAttribute("is_officer")]
	public bool IsOfficer { get; set; }
}
