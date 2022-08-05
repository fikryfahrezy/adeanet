namespace Adea.Services.UserService;

public record RegisterRequestBodyDTO
{
	public bool IsOfficer { get; set; }
	public string? Username { get; set; }
	public string? Password { get; set; }
}

public record RegisterResponseBodyDTO
{
	public bool? IsOfficer { get; set; }
	public string? Id { get; set; }
}

public record LoginRequestBodyDTO
{
	public string? Username { get; set; }
	public string? Password { get; set; }
}

public record LoginResponseBodyDTO
{
	public bool IsOfficer { get; set; }
	public string? Id { get; set; }
}

public record UserResponseBodyDTO
{
	public string? Id { get; set; }
	public string? Username { get; set; }
	public bool? IsOfficer { get; set; }
}