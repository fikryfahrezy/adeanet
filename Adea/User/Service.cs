using Adea.Common;
using Adea.Controllers;
using Adea.Exceptions;

namespace Adea.User;

public class UserService
{

    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    private async Task CheckUsernameExistAndThrowAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user != null)
        {
            throw new UnprocessableEntityException($"Username {username} already exist");
        }
    }

    private async Task<Member> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null)
        {
            throw new NotFoundException($"Username {username} not exist");
        }

        return user;
    }

    private static void VerifyUserPasswordAndThrow(string requestPassword, string userPassword)
    {
        if (!Argon2.Verify(requestPassword, userPassword))
        {
            throw new UnauthorizedException("Password not match");
        }
    }

    public async Task<RegisterResponseBodyDTO> RegisterUserAsync(RegisterUser request)
    {
        await CheckUsernameExistAndThrowAsync(request.Username);

        var hashedPassword = Argon2.Hash(
            type: Argon2Type.ID,
            word: request.Password,
            keyLen: 16,
            config: null
        );


        var newUserID = await _userRepository.InsertUserAsync(new RegisterUser(
            username: request.Username,
            password: hashedPassword,
            isOfficer: request.IsOfficer
        ));

        return new RegisterResponseBodyDTO
        {
            Id = newUserID,
            IsOfficer = request.IsOfficer
        };
    }

    public async Task<LoginResponseBodyDTO> LoginUserAsync(LoginUser user)
    {
        var existingUser = await GetUserByUsernameAsync(user.Username);
        VerifyUserPasswordAndThrow(user.Password, existingUser.Password);

        return new LoginResponseBodyDTO
        {
            Id = existingUser.Id,
            IsOfficer = existingUser.IsOfficer
        };
    }
}