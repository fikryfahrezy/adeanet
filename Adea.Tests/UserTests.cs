
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Adea.Services.UserService;

namespace Adea.Tests;

public class UserServicesTests : IDisposable
{
	private readonly DbConnection _connection;
	private readonly DbContextOptions<LoanLosDbContext> _contextOptions;

	#region ConstructorAndDispose
	public UserServicesTests()
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

	[Fact]
	public async Task GetAllBlogs()
	{
		using var context = CreateContext();
		var repository = new UserRepository(context);
		var services = new UserServices(repository);

		var user = new RegisterRequestBodyDTO
		{
			Username = "username",
			Password = "password",
			IsOfficer = true,
		};

		var serviceResponse = await services.SaveUser(user);
		Assert.NotEmpty(serviceResponse.Id);
	}
}