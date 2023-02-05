using Adea.Common;
using Adea.Models;
using Adea.Exceptions;

namespace Adea.User;

public class UserService
{

	private readonly UserRepository _userRepository;

	public UserService(UserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<RegisterResponseBodyDTO> SaveUserAsync(RegisterRequestBodyDTO request)
	{
		await _userRepository.CheckUsernameExistAndThrowAsync(request.Username);

		var hashedPassword = Argon2.Hash(
			type: Argon2Type.ID,
			word: request.Password,
			keyLen: 16,
			config: null
		);

		var user = new UserDAO
		{
			Username = request.Username,
			Password = hashedPassword,
			IsOfficer = request.IsOfficer
		};

		await _userRepository.InsertUserAsync(user);

		return new RegisterResponseBodyDTO
		{
			Id = user.Id,
			IsOfficer = user.IsOfficer
		};
	}

	public async Task<LoginResponseBodyDTO> VerifyUserAsync(LoginRequestBodyDTO request)
	{
		var user = await _userRepository.GetUserByUsernameAsync(request.Username);
		Argon2.VerifyAndThrow(request.Password, user.Password);

		return new LoginResponseBodyDTO
		{
			Id = user.Id,
			IsOfficer = user.IsOfficer
		};
	}
}