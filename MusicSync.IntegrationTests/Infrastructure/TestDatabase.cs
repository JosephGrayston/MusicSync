using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MusicSync.Infrastructure.Persistence;

namespace MusicSync.IntegrationTests.Infrastructure;

public sealed class TestDatabase : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<MusicSyncDbContext> _options;

    private TestDatabase(SqliteConnection connection)
    {
        _connection = connection;

        _options = new DbContextOptionsBuilder<MusicSyncDbContext>()
            .UseSqlite(connection)
            .Options;
    }

    public static async Task<TestDatabase> CreateAsync(CancellationToken cancellationToken)
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync(cancellationToken);

        var database = new TestDatabase(connection);

        await using var context = database.CreateContext();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        return database;
    }

    public MusicSyncDbContext CreateContext() => new(_options);

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}
