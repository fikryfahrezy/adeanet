using Microsoft.AspNetCore.Mvc;
using Adea.User;
using FluentValidation;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Adea.Options;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.X86;

namespace Adea.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IOptions<AppEnvOptions> _config;

    public AuthController(UserService userService, IOptions<AppEnvOptions> config)
    {
        _userService = userService;
        _config = config;
    }

    private string GenerateJWT(string userId)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, userId)
        };
        Console.WriteLine(_config.Value.Jwt.IssuerSigningKey);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Jwt.IssuerSigningKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            issuer: _config.Value.Jwt.ValidIssuer,
            audience: _config.Value.Jwt.ValidAudiences.FirstOrDefault(),
            notBefore: DateTime.Now,
            signingCredentials: cred
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private AuthResponseBodyDTO AuthResponse(string userId) => new()
    {
        Token = GenerateJWT(userId),
    };

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseBodyDTO>> RegisterAsync([FromBody] RegisterRequestBodyDTO requestBody)
    {
        var validator = new RegisterRequestBodyDTOValidator();
        await validator.ValidateAndThrowAsync(requestBody);

        var registerUser = new RegisterUser(requestBody.Username, requestBody.Password, requestBody.IsOfficer);
        var registeredUser = await _userService.RegisterUserAsync(registerUser);

        return AuthResponse(registeredUser.Id);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseBodyDTO>> LoginAsync([FromBody] LoginRequestBodyDTO requestBody)
    {
        var validator = new LoginRequestBodyDTODTOValidator();
        await validator.ValidateAndThrowAsync(requestBody);

        var loginUser = new LoginUser(requestBody.Username, requestBody.Password);
        var loggedInUser = await _userService.LoginUserAsync(loginUser);

        return AuthResponse(loggedInUser.Id);
    }
}