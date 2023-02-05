using Microsoft.AspNetCore.Mvc;
using Adea.User;
using FluentValidation;

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
        var validator = new RegisterRequestBodyDTOValidator();
        await validator.ValidateAndThrowAsync(requestBody);
        return await _userService.SaveUserAsync(requestBody);
	}

	[HttpPost("login")]
	public async Task<ActionResult<LoginResponseBodyDTO>> LoginAsync([FromBody] LoginRequestBodyDTO requestBody)
	{
        var validator = new LoginRequestBodyDTODTOValidator();
        await validator.ValidateAndThrowAsync(requestBody);
        return await _userService.VerifyUserAsync(requestBody);
	}
}