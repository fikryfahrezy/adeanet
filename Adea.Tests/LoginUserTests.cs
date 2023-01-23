
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using NUnit.Framework;
using Adea.Data;
using Adea.Exceptions;
using Adea.User;

namespace Adea.Tests;

public class LoginUserTests : IDisposable
{
	private DbConnection _connection;
	private DbContextOptions<LoanLosDbContext> _contextOptions;

	#region ConstructorAndDispose
	public LoginUserTests()
	{
		// Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
		// at the end of the test (see Dispose below).
		_connection = new SqliteConnection("Filename=:memory:");
		_connection.Open();

		// These options will be used by the context instances in this test suite, including the connection opened above.
		_contextOptions = new DbContextOptionsBuilder<LoanLosDbContext>()
			.UseSqlite(_connection)
			.Options;

		// Create the schema and seed some data
		using var context = new LoanLosDbContext(_contextOptions);
		context.Database.EnsureDeleted();
		context.Database.EnsureCreated();
	}

	LoanLosDbContext CreateContext() => new LoanLosDbContext(_contextOptions);

	public void Dispose() => _connection.Dispose();
	#endregion

	[Test(Description = "Login successfully")]
	public async Task VerifyUser_PropperData_Success()
	{
		using var context = CreateContext();

		var repository = new UserRepository(context);
		var service = new UserService(repository);

		var user = new RegisterRequestBodyDTO
		{
			Username = "username",
			Password = "password",
			IsOfficer = true,
		};

		var savedUser = await service.SaveUser(user);
		Assert.NotNull(savedUser.Id);


		var identity = new LoginRequestBodyDTO
		{
			Username = "username",
			Password = "password",
		};

		var verifiedUser = await service.VerifyUser(identity);
		Assert.NotNull(verifiedUser.Id);
		Assert.AreEqual(savedUser.Id, verifiedUser.Id);

		await context.Database.ExecuteSqlRawAsync("DELETE FROM users");
	}

	static object[] VerifyUserFailCases =
	{
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

	[TestCaseSource(nameof(VerifyUserFailCases))]
	public async Task VerifyUser_WrongCredential_Fail(Type type, LoginRequestBodyDTO request)
	{
		using var context = CreateContext();

		var repository = new UserRepository(context);
		var service = new UserService(repository);

		var user = new RegisterRequestBodyDTO
		{
			Username = "username",
			Password = "password",
			IsOfficer = true,
		};

		var savedUser = await service.SaveUser(user);
		Assert.NotNull(savedUser.Id);

		Assert.ThrowsAsync(type, async () => await service.VerifyUser(request));

		await context.Database.ExecuteSqlRawAsync("DELETE FROM users");
	}


	static object[] VerifyUserValidationCases =
	{
		// Login fail, no input provided
		new object[] {
			new LoginRequestBodyDTO {
			},
		},
		// Login fail, no username provided
		new object[] {
			new LoginRequestBodyDTO {
				Password = "password",
			},
		},
		// Login fail, no password provided
		new object[] {
			new LoginRequestBodyDTO {
				Username = "username",
			},
		},
	};

	[TestCaseSource(nameof(VerifyUserValidationCases))]
	public async Task VerifyUser_Validation_Fail(LoginRequestBodyDTO request)
	{
		using var context = CreateContext();

		var repository = new UserRepository(context);
		var service = new UserService(repository);

		Assert.ThrowsAsync(typeof(ValidationException), async () => await service.VerifyUser(request));

		await context.Database.ExecuteSqlRawAsync("DELETE FROM users");
	}
}