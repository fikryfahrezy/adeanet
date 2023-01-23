using Microsoft.AspNetCore.Mvc;
using Adea.User;

namespace Adea.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly UserService _userService;

	public UsersController(UserService userService)
	{
		_userService = userService;
	}

	[HttpPost("register")]
	public async Task<ActionResult<RegisterResponseBodyDTO>> Register([FromBody] RegisterRequestBodyDTO requestBody)
	{
		return await _userService.SaveUser(requestBody);
	}

	[HttpPost("login")]
	public async Task<ActionResult<LoginResponseBodyDTO>> Login([FromBody] LoginRequestBodyDTO requestBody)
	{
		return await _userService.VerifyUser(requestBody);
	}
}