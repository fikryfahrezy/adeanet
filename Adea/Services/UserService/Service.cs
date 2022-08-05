using Adea.Common;
using Adea.Models;

namespace Adea.Services.UserService;

public class UserServices
{

	private readonly UserRepository _repository;

	public UserServices(UserRepository repository)
	{
		_repository = repository;
	}

	public async Task<RegisterResponseBodyDTO> SaveUser(RegisterRequestBodyDTO request)
	{
		var hashedPassword = Argon2.Hash(
			type: Argon2Type.ID,
			word: request.Password,
			bytes: 16,
			config: new Argon2Param()
		);

		var user = new User
		{
			Username = request.Username,
			IsOfficer = request.IsOfficer,
			Password = hashedPassword,
		};

		await _repository.InsertUserAsync(user);

		return new RegisterResponseBodyDTO
		{
			Id = user.Id,
			IsOfficer = user.IsOfficer,
		};
	}

	public async Task<List<UserResponseBodyDTO>> GerUsers()
	{
		var users = await _repository.GetUsersAsync();

		return users.ConvertAll<UserResponseBodyDTO>(u => new UserResponseBodyDTO
		{
			Id = u.Id,
			IsOfficer = u.IsOfficer,
			Username = u.Username
		});
	}

}