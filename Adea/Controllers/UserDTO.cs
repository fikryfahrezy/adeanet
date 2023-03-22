using System.Text.Json.Serialization;

namespace Adea.Controllers;

public class RegisterRequestBodyDTO
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = "";

    [JsonPropertyName("password")]
    public string Password { get; set; } = "";

    [JsonPropertyName("is_officer")]
    public bool IsOfficer { get; set; }
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
    [JsonPropertyName("username")]
    public string Username { get; set; } = "";

    [JsonPropertyName("password")]
    public string Password { get; set; } = "";
}

public class LoginResponseBodyDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("is_officer")]
    public bool IsOfficer { get; set; }
}

public class AuthResponseBodyDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = "";
}
