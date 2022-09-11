
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using NUnit.Framework;
using Adea.Data;
using Adea.Exceptions;
using Adea.Services.UserService;

namespace Adea.Tests;

[TestFixture]
public class LoginUserTests
{
	private DbConnection _connection;
	private DbContextOptions<LoanLosDbContext> _contextOptions;

	[OneTimeSetUp]
	public void Init()
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

	[Test]
	public async Task VerifyUser_PropperData_Success()
	{
		using var context = CreateContext();
		await context.Database.ExecuteSqlRawAsync("DELETE FROM users");

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
	}

	[Test]
	public async Task VerifyUser_PropperData_PasswordNotMatch()
	{
		using var context = CreateContext();
		await context.Database.ExecuteSqlRawAsync("DELETE FROM users");

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
			Password = "passwordx",
		};

		Assert.ThrowsAsync(typeof(UnauthorizedException), async () => await service.VerifyUser(identity));
	}

	[Test]
	public async Task VerifyUser_PropperData_NotFound()
	{
		using var context = CreateContext();
		await context.Database.ExecuteSqlRawAsync("DELETE FROM users");

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
			Username = "usernamex",
			Password = "password",
		};

		Assert.ThrowsAsync(typeof(NotFoundException), async () => await service.VerifyUser(identity));
	}

	static object[] SaveUserValidationCases =
	{
		new object[] {
			new LoginRequestBodyDTO {
			},
		},
		new object[] {
			new LoginRequestBodyDTO {
				Password = "password",
			},
		},
		new object[] {
			new LoginRequestBodyDTO {
				Username = "username",
			},
		},
	};

	[TestCaseSource(nameof(SaveUserValidationCases))]
	public async Task SaveUser_Validation_Fail(LoginRequestBodyDTO request)
	{
		using var context = CreateContext();
		await context.Database.ExecuteSqlRawAsync("DELETE FROM users");

		var repository = new UserRepository(context);
		var service = new UserService(repository);

		Assert.ThrowsAsync(typeof(ValidationException), async () => await service.VerifyUser(request));
	}
}