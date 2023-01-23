using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Adea.User;

public record RegisterRequestBodyDTO
{
	[BindProperty(Name = "username")]
	public string Username { get; set; } = "";

	[BindProperty(Name = "password")]
	public string Password { get; set; } = "";

	[BindProperty(Name = "is_officer")]
	public bool IsOfficer { get; set; } = false;
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
	[BindProperty(Name = "username")]
	public string Username { get; set; } = "";

	[BindProperty(Name = "password")]
	public string Password { get; set; } = "";
}

public record LoginResponseBodyDTO
{
	[JsonPropertyNameAttribute("id")]
	public string Id { get; set; } = "";

	[JsonPropertyNameAttribute("is_officer")]
	public bool IsOfficer { get; set; }
}
