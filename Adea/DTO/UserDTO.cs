using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Adea.DTO;

public class RegisterRequestBodyDTO
{
    [BindProperty(Name = "username")]
    public string Username { get; set; } = "";

    [BindProperty(Name = "password")]
    public string Password { get; set; } = "";

    [BindProperty(Name = "is_officer")]
    public bool IsOfficer { get; set; } = false;
}

public class RegisterResponseBodyDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("is_officer")]
    public bool IsOfficer { get; set; }
}

public class LoginRequestBodyDTO
{
    [BindProperty(Name = "username")]
    public string Username { get; set; } = "";

    [BindProperty(Name = "password")]
    public string Password { get; set; } = "";
}

public class LoginResponseBodyDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("is_officer")]
    public bool IsOfficer { get; set; }
}
