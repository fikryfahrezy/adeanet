
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using NUnit.Framework;
using Adea.Services.Data;
using Adea.Services.Exceptions;
using Adea.Services.User;

namespace Adea.Tests;

[TestFixture]
public class RegisterUserTests
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

	static object[] SuccessSaveUserCases =
	{
		new object[] {
			new RegisterRequestBodyDTO {
				Username = "nonexsistofficerusername",
				Password = "password",
				IsOfficer = true,
			},
		},
		new object[] {
			new RegisterRequestBodyDTO {
				Username = "nonexsistnonofficerusername",
				Password = "password",
				IsOfficer = false,
			},
		},
	};

	[TestCaseSource(nameof(SuccessSaveUserCases))]
	public async Task SaveUser_ProperData_Success(RegisterRequestBodyDTO request)
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

		var firstServiceResponse = await service.SaveUser(user);
		Assert.NotNull(firstServiceResponse.Id);

		var secondServiceResponse = await service.SaveUser(request);
		Assert.NotNull(secondServiceResponse.Id);
	}

	[Test]
	public async Task SaveUser_DuplicateUsername_Fail()
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

		var serviceResponse = await service.SaveUser(user);
		Assert.NotNull(serviceResponse.Id);
		Assert.ThrowsAsync(typeof(UnprocessableEntityException), async () => await service.SaveUser(user));
	}

	static object[] SaveUserValidationCases =
	{
		new object[] {
			new RegisterRequestBodyDTO {
				IsOfficer = true,
			},
		},
		new object[] {
			new RegisterRequestBodyDTO {
				Password = "password",
				IsOfficer = true,
			},
		},
		new object[] {
			new RegisterRequestBodyDTO {
				Username = "username",
				IsOfficer = true,
			},
		},
	};

	[TestCaseSource(nameof(SaveUserValidationCases))]
	public async Task SaveUser_Validation_Fail(RegisterRequestBodyDTO request)
	{
		using var context = CreateContext();
		await context.Database.ExecuteSqlRawAsync("DELETE FROM users");

		var repository = new UserRepository(context);
		var service = new UserService(repository);

		Assert.ThrowsAsync(typeof(ValidationException), async () => await service.SaveUser(request));
	}
}