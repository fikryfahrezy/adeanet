using FluentValidation;
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

	public async Task<RegisterResponseBodyDTO> SaveUser(RegisterRequestBodyDTO request)
	{
		var validator = new RegisterRequestBodyDTOValidator();
		await validator.ValidateAndThrowAsync(request);

		var existUser = await _userRepository.GetUserByUsernameAsync(request.Username);

		if (existUser != null)
		{
			throw new UnprocessableEntityException($"Username {request.Username} already exist");
		}

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

	public async Task<LoginResponseBodyDTO> VerifyUser(LoginRequestBodyDTO request)
	{
		var validator = new LoginRequestBodyDTODTOValidator();
		await validator.ValidateAndThrowAsync(request);

		var user = await _userRepository.GetUserByUsernameAsync(request.Username);

		if (user == null)
		{
			throw new NotFoundException($"Username {request.Username} not exist");
		}

		var isPasswordMatch = Argon2.Verify(request.Password, user.Password);

		if (!isPasswordMatch)
		{
			throw new UnauthorizedException("Password not match");
		}

		return new LoginResponseBodyDTO
		{
			Id = user.Id,
			IsOfficer = user.IsOfficer
		};
	}
}