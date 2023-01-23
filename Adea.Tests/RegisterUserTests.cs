
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using NUnit.Framework;
using Adea.Data;
using Adea.Exceptions;
using Adea.User;

namespace Adea.Tests;

public class RegisterUserTests : IDisposable
{
	private DbConnection _connection;
	private DbContextOptions<LoanLosDbContext> _contextOptions;

	#region ConstructorAndDispose
	public RegisterUserTests()
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

	static object[] SuccessSaveUserCases =
	{
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

	[TestCaseSource(nameof(SuccessSaveUserCases))]
	public async Task SaveUser_ProperData_Success(RegisterRequestBodyDTO request)
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

		var firstServiceResponse = await service.SaveUser(user);
		Assert.NotNull(firstServiceResponse.Id);

		var secondServiceResponse = await service.SaveUser(request);
		Assert.NotNull(secondServiceResponse.Id);

		await context.Database.ExecuteSqlRawAsync("DELETE FROM users");
	}

	[Test(Description = "Register fail, username exist")]
	public async Task SaveUser_DuplicateUsername_Fail()
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

		var serviceResponse = await service.SaveUser(user);
		Assert.NotNull(serviceResponse.Id);
		Assert.ThrowsAsync(typeof(UnprocessableEntityException), async () => await service.SaveUser(user));

		await context.Database.ExecuteSqlRawAsync("DELETE FROM users");
	}

	static object[] SaveUserValidationCases =
	{
		// Register fail, no input provided
		new object[] {
			new RegisterRequestBodyDTO {
			},
		},
		// Register fail, no username provided
		new object[] {
			new RegisterRequestBodyDTO {
				Password = "password",
			},
		},
		// Register fail, no password provided
		new object[] {
			new RegisterRequestBodyDTO {
				Username = "username",
			},
		},
	};

	[TestCaseSource(nameof(SaveUserValidationCases))]
	public async Task SaveUser_Validation_Fail(RegisterRequestBodyDTO request)
	{
		using var context = CreateContext();

		var repository = new UserRepository(context);
		var service = new UserService(repository);

		Assert.ThrowsAsync(typeof(ValidationException), async () => await service.SaveUser(request));

		await context.Database.ExecuteSqlRawAsync("DELETE FROM users");
	}
}