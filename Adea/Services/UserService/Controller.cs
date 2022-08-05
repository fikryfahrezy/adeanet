using Microsoft.AspNetCore.Mvc;

namespace Adea.Services.UserService;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly UserServices _service;

	public UsersController(UserServices service)
	{
		_service = service;
	}

	[HttpPost("register")]
	public async Task<ActionResult<RegisterResponseBodyDTO>> Register(RegisterRequestBodyDTO requestBody)
	{
		return await _service.SaveUser(requestBody);
	}

	[HttpPost("login")]
	public async Task<ActionResult<LoginResponseBodyDTO>> Login(LoginRequestBodyDTO requestBody)
	{
		return await Task.Run(() => new LoginResponseBodyDTO());
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<UserResponseBodyDTO>>> GetUsers()
	{
		return await _service.GerUsers();
	}
}