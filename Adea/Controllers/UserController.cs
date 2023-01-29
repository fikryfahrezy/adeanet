using Microsoft.AspNetCore.Mvc;
using Adea.User;

namespace Adea.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly UserService _userService;

	public AuthController(UserService userService)
	{
		_userService = userService;
	}

	[HttpPost("register")]
	public async Task<ActionResult<RegisterResponseBodyDTO>> RegisterAsync([FromBody] RegisterRequestBodyDTO requestBody)
	{
		return await _userService.SaveUserAsync(requestBody);
	}

	[HttpPost("login")]
	public async Task<ActionResult<LoginResponseBodyDTO>> LoginAsync([FromBody] LoginRequestBodyDTO requestBody)
	{
		return await _userService.VerifyUserAsync(requestBody);
	}
}