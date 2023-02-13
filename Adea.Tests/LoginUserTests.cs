
using Xunit;
using Adea.Exceptions;
using Adea.User;

namespace Adea.Tests;

public class LoginUserTests : IClassFixture<DatabaseFixture>
{
    readonly DatabaseFixture _databaseFixture;
    public LoginUserTests(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

    [Fact(DisplayName = "Login successfully")]
    public async Task VerifyUser_PropperData_Success()
    {
        using var context = _databaseFixture.CreateContext();

        var repository = new UserRepository(context);
        var service = new UserService(repository);

        var user = new RegisterRequestBodyDTO
        {
            Username = "username",
            Password = "password",
            IsOfficer = true,
        };

        var savedUser = await service.SaveUserAsync(user);
        Assert.NotNull(savedUser.Id);


        var identity = new LoginRequestBodyDTO
        {
            Username = "username",
            Password = "password",
        };

        var verifiedUser = await service.VerifyUserAsync(identity);
        Assert.NotEmpty(verifiedUser.Id);
        Assert.Equal(savedUser.Id, verifiedUser.Id);

        await _databaseFixture.ClearDB(context);
    }

    public static IEnumerable<object[]> VerifyUserFailCases
        => new object[][] {
			// Login fail, password not match
			new object[] {
                typeof(UnauthorizedException),
                new LoginRequestBodyDTO
                {
                    Username = "username",
                    Password = "passwordx",
                },
            },
			// Login fail, user not found
			new object[] {
                typeof(NotFoundException),
                new LoginRequestBodyDTO
                {
                    Username = "usernamex",
                    Password = "password",
                },
            },
        };

    [Theory]
    [MemberData(nameof(VerifyUserFailCases))]
    public async Task VerifyUser_WrongCredential_Fail(Type type, LoginRequestBodyDTO request)
    {
        using var context = _databaseFixture.CreateContext();

        var repository = new UserRepository(context);
        var service = new UserService(repository);

        var user = new RegisterRequestBodyDTO
        {
            Username = "username",
            Password = "password",
            IsOfficer = true,
        };

        var savedUser = await service.SaveUserAsync(user);
        Assert.NotEmpty(savedUser.Id);

        await Assert.ThrowsAsync(type, async () => await service.VerifyUserAsync(request));

        await _databaseFixture.ClearDB(context);
    }
}