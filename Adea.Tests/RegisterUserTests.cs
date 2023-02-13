
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
				new RegisterRequestBodyDTO {
					Username = "nonexsistofficerusername",
					Password = "password",
					IsOfficer = true,
				},
			},
			// Register as non officer successfully
			new object[] {
				new RegisterRequestBodyDTO {
					Username = "nonexsistnonofficerusername",
					Password = "password",
					IsOfficer = false,
				},
			},
		};

	[Theory]
	[MemberData(nameof(SuccessSaveUserCases))]
	public async Task SaveUser_ProperData_Success(RegisterRequestBodyDTO request)
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

		var firstServiceResponse = await service.SaveUserAsync(user);
		Assert.NotEmpty(firstServiceResponse.Id);

		var secondServiceResponse = await service.SaveUserAsync(request);
		Assert.NotEmpty(secondServiceResponse.Id);

        await _databaseFixture.ClearDB(context);
    }

	[Fact(DisplayName = "Register fail, username exist")]
	public async Task SaveUser_DuplicateUsername_Fail()
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

		var serviceResponse = await service.SaveUserAsync(user);
		Assert.NotEmpty(serviceResponse.Id);
		await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.SaveUserAsync(user));

		await _databaseFixture.ClearDB(context);
	}
}