using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using MusicSync.IntegrationTests.Infrastructure;
using MusicSync.Playlists;
using Shouldly;

namespace MusicSync.IntegrationTests.Playlists;

[TestClass]
public sealed class PlaylistStoreTests
{
    private static readonly DateTimeOffset _now = new(2026, 11, 23, 9, 30, 0, TimeSpan.Zero);

    public TestContext TestContext { get; set; }


    [TestMethod]
    public async Task CreateAsyncStampsCreatedAtFromFakeTimeProvider()
    {
        var cancellationToken = TestContext.CancellationToken;
        await using var database = await TestDatabase.CreateAsync(cancellationToken);
        var time = new FakeTimeProvider(_now);


        await using (var context = database.CreateContext())
        {
            await new PlaylistStore(context, time).CreateAsync("Hip Hop", cancellationToken);
        }

        await using var readContext = database.CreateContext();
        var saved = await readContext.Set<Playlist>().SingleAsync(cancellationToken);
 
        saved.Name.ShouldBe("Hip Hop");
        saved.CreatedAt.ShouldBe(_now);
    }

    [TestMethod]
    public async Task CreateAsyncDuplicateNameIgnoringCaseThrows()
    {
        var cancellationToken = TestContext.CancellationToken;
        await using var database = await TestDatabase.CreateAsync(cancellationToken);
        var time = new FakeTimeProvider(_now);

        await using (var firstContext = database.CreateContext())
        {
            await new PlaylistStore(firstContext, time).CreateAsync("Hip Hop", cancellationToken);
        }

        await using var secondContext = database.CreateContext();

        await Should.ThrowAsync<DbUpdateException>(() => new PlaylistStore(secondContext, time).CreateAsync("hip hop", cancellationToken));
    }

    [TestMethod]
    public async Task GetAllAsyncOrdersNameCaseInsensitive()
    {
        var cancellationToken = TestContext.CancellationToken;
        await using var database = await TestDatabase.CreateAsync(cancellationToken);
        var time = new FakeTimeProvider(_now);

        foreach (var name in new[] {"Rock", "drum & bass", "Jazz", "pop"})
        {
            await using var context = database.CreateContext();
            await new PlaylistStore(context, time).CreateAsync(name, cancellationToken);
        }

        await using var readContext = database.CreateContext();

        var playlists = await new PlaylistStore(readContext, time).GetAllAsync(cancellationToken);

        playlists.Select(p => p.Name).ShouldBe(["drum & bass", "Jazz", "pop", "Rock" ]);
    }

    
}
