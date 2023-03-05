
using Xunit;
using Adea.Exceptions;
using Adea.User;

namespace Adea.Tests;

public class RegisterUserTests : IClassFixture<DatabaseFixture>
{
    readonly DatabaseFixture _databaseFixture;
    public RegisterUserTests(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

    public static IEnumerable<object[]> SuccessSaveUserCases
        => new object[][] {
			// Register as officer successfully
			new object[] {
                new RegisterUser("nonexsistofficerusername", "password", true),
            },
			// Register as non officer successfully
			new object[] {
                new RegisterUser("nonexsistnonofficerusername", "password", false),
            },
        };

    [Theory]
    [MemberData(nameof(SuccessSaveUserCases))]
    public async Task SaveUser_ProperData_Success(RegisterUser request)
    {
        using var context = _databaseFixture.CreateContext();

        var repository = new UserRepository(context);
        var service = new UserService(repository);

        var user = new RegisterUser("username", "password", true);
        var firstServiceResponse = await service.RegisterUserAsync(user);

        Assert.NotEmpty(firstServiceResponse.Id);

        var secondServiceResponse = await service.RegisterUserAsync(request);
        Assert.NotEmpty(secondServiceResponse.Id);

        await _databaseFixture.ClearDB(context);
    }

    [Fact(DisplayName = "Register fail, username exist")]
    public async Task SaveUser_DuplicateUsername_Fail()
    {
        using var context = _databaseFixture.CreateContext();

        var repository = new UserRepository(context);
        var service = new UserService(repository);

        var user = new RegisterUser("username", "password", true);

        var serviceResponse = await service.RegisterUserAsync(user);
        Assert.NotEmpty(serviceResponse.Id);
        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.RegisterUserAsync(user));

        await _databaseFixture.ClearDB(context);
    }
}