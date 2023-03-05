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

        var user = new RegisterUser(
            username: "username",
            password: "password",
            isOfficer: true
        );

        var savedUser = await service.RegisterUserAsync(user);
        Assert.NotNull(savedUser.Id);

        var identity = new LoginUser("username", "password");
        var verifiedUser = await service.LoginUserAsync(identity);

        Assert.NotEmpty(verifiedUser.Id);
        Assert.Equal(savedUser.Id, verifiedUser.Id);

        await _databaseFixture.ClearDB(context);
    }

    public static IEnumerable<object[]> VerifyUserFailCases
        => new object[][] {
			// Login fail, password not match
			new object[] {
                typeof(UnauthorizedException),
                new LoginUser("username", "passwordx")
            },
			// Login fail, user not found
			new object[] {
                typeof(NotFoundException),
                new LoginUser("usernamex", "password"),
            },
        };

    [Theory]
    [MemberData(nameof(VerifyUserFailCases))]
    public async Task VerifyUser_WrongCredential_Fail(Type type, LoginUser request)
    {
        using var context = _databaseFixture.CreateContext();

        var repository = new UserRepository(context);
        var service = new UserService(repository);

        var user = new RegisterUser(
             username: "username",
             password: "password",
             isOfficer: true
         );

        var savedUser = await service.RegisterUserAsync(user);
        Assert.NotEmpty(savedUser.Id);

        await Assert.ThrowsAsync(type, async () => await service.LoginUserAsync(request));

        await _databaseFixture.ClearDB(context);
    }
}