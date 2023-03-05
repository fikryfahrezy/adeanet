using Microsoft.AspNetCore.Mvc;
using Adea.DTO;
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

        var registerUser = new RegisterUser(requestBody.Username, requestBody.Password, requestBody.IsOfficer);
        return await _userService.RegisterUserAsync(registerUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseBodyDTO>> LoginAsync([FromBody] LoginRequestBodyDTO requestBody)
    {
        var validator = new LoginRequestBodyDTODTOValidator();
        await validator.ValidateAndThrowAsync(requestBody);

        var loginUser = new LoginUser(requestBody.Username, requestBody.Password);
        return await _userService.LoginUserAsync(loginUser);
    }
}