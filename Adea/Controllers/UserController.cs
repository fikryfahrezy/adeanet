using Microsoft.AspNetCore.Mvc;
using Adea.User;
using FluentValidation;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Adea.Options;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

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

    private string GenerateJWT(string userId, bool isAdmin)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
        };

        if (isAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

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

    private AuthResponseBodyDTO AuthResponse(string userId, bool isAdmin) => new()
    {
        Token = GenerateJWT(userId, isAdmin),
    };

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseBodyDTO>> RegisterAsync([FromBody] RegisterRequestBodyDTO requestBody)
    {
        var validator = new RegisterRequestBodyDTOValidator();
        await validator.ValidateAndThrowAsync(requestBody);

        var registerUser = new RegisterUser(requestBody.Username, requestBody.Password, requestBody.IsOfficer);
        var registeredUser = await _userService.RegisterUserAsync(registerUser);

        return AuthResponse(registeredUser.Id, registeredUser.IsOfficer);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseBodyDTO>> LoginAsync([FromBody] LoginRequestBodyDTO requestBody)
    {
        var validator = new LoginRequestBodyDTODTOValidator();
        await validator.ValidateAndThrowAsync(requestBody);

        var loginUser = new LoginUser(requestBody.Username, requestBody.Password);
        var loggedInUser = await _userService.LoginUserAsync(loginUser);

        return AuthResponse(loggedInUser.Id, loggedInUser.IsOfficer);
    }
}