using Adea.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Adea.Tests;

public class DatabaseFixture : IDisposable
{
    private DbConnection _connection;
    private DbContextOptions<LoanLosDbContext> _contextOptions;

    public DatabaseFixture()
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

    public void Dispose() => _connection.Dispose();

    public LoanLosDbContext CreateContext() => new(_contextOptions);

    public async Task ClearDB(LoanLosDbContext context)
    {
        await context.Database.ExecuteSqlRawAsync("DELETE FROM loan_applications");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM users");
    }

}

