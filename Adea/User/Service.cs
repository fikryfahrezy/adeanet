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
        await CheckUsernameExistAndThrowAsync(request.Username);

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

    private async Task CheckUsernameExistAndThrowAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user != null)
        {
            throw new UnprocessableEntityException($"Username {username} already exist");
        }
    }

    private async Task<UserDAO> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null)
        {
            throw new NotFoundException($"Username {username} not exist");
        }

        return user;
    }

    public async Task<LoginResponseBodyDTO> VerifyUserAsync(LoginRequestBodyDTO request)
    {
        var user = await GetUserByUsernameAsync(request.Username);
        VerifyUserPasswordAndThrow(request.Password, user.Password);

        return new LoginResponseBodyDTO
        {
            Id = user.Id,
            IsOfficer = user.IsOfficer
        };
    }

    private static void VerifyUserPasswordAndThrow(string requestPassword, string userPassword)
    {
        if (!Argon2.Verify(requestPassword, userPassword))
        {
            throw new UnauthorizedException("Password not match");
        }
    }
}