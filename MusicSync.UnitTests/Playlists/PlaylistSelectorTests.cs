using MusicSync.Playlists;
using NSubstitute;
using Shouldly;

namespace MusicSync.UnitTests.Playlists;

[TestClass]
public sealed class PlaylistSelectorTests
{
    private static readonly DateTimeOffset _now = new(2026, 11, 23, 9, 30, 0, TimeSpan.Zero);

    public TestContext TestContext { get; set; }

    [TestMethod]
    public async Task SelectAsyncWithNoExistingPlaylistsSkipsActionPromptAndCreates()
    {
        var prompts = Substitute.For<IPlaylistPrompts>();

        prompts.AskForNewPlaylistNameAsync(Arg.Is<IReadOnlySet<string>>(names => names.Count == 0), TestContext.CancellationToken)
            .Returns("Study Playlist");

        var store = Substitute.For<IPlaylistStore>();

        store.GetAllAsync(TestContext.CancellationToken)
            .Returns([]);

        store.CreateAsync("Study Playlist", TestContext.CancellationToken)
            .Returns(new Playlist { Id = 1, Name = "Study Playlist", CreatedAt = _now });

        var selector = new PlaylistSelector(prompts, store);

        var selectedId = await selector.SelectAsync(TestContext.CancellationToken);

        selectedId.ShouldBe(1);
        await prompts.DidNotReceive().AskForPlaylistActionAsync(TestContext.CancellationToken);
        await prompts.Received().AskForNewPlaylistNameAsync(Arg.Is<IReadOnlySet<string>>(names => names.Count == 0), TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task SelectAsyncWithExistingPlaylistsPromptsForActionAndSelectsExisting()
    {
        var existingPlaylists = new List<Playlist>
        {
            new() { Id = 1, Name = "Chill Vibes", CreatedAt = _now },
            new() { Id = 2, Name = "Workout Mix", CreatedAt = _now }
        };

        var prompts = Substitute.For<IPlaylistPrompts>();

        prompts.AskForPlaylistActionAsync(TestContext.CancellationToken)
            .Returns(PlaylistAction.SelectExistingPlaylist);

        prompts.ChoosePlaylistAsync(existingPlaylists, TestContext.CancellationToken)
            .Returns(existingPlaylists[1]);

        var store = Substitute.For<IPlaylistStore>();

        store.GetAllAsync(TestContext.CancellationToken)
            .Returns(existingPlaylists);

        var selector = new PlaylistSelector(prompts, store);

        var selectedId = await selector.SelectAsync(TestContext.CancellationToken);

        selectedId.ShouldBe(2);
        await prompts.Received().AskForPlaylistActionAsync(TestContext.CancellationToken);
        await prompts.Received().ChoosePlaylistAsync(existingPlaylists, TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task SelectAsyncWithExistingPlaylistPromptsForActionAndCreatesNew()
    {
        var existingPlaylists = new List<Playlist>
        {
            new() { Id = 1, Name = "Chill Vibes", CreatedAt = _now },
            new() { Id = 2, Name = "Workout Mix", CreatedAt = _now }
        };

        var expectedNames = existingPlaylists.Select(p => p.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var prompts = Substitute.For<IPlaylistPrompts>();

        prompts.AskForPlaylistActionAsync(TestContext.CancellationToken)
            .Returns(PlaylistAction.CreateNewPlaylist);

        prompts.AskForNewPlaylistNameAsync(Arg.Is<IReadOnlySet<string>>(names => names.SetEquals(expectedNames)), TestContext.CancellationToken)
            .Returns("Study Playlist");

        var store = Substitute.For<IPlaylistStore>();

        store.GetAllAsync(TestContext.CancellationToken)
            .Returns(existingPlaylists);

        store.CreateAsync("Study Playlist", TestContext.CancellationToken)
            .Returns(new Playlist { Id = 3, Name = "Study Playlist", CreatedAt = _now });


        var selector = new PlaylistSelector(prompts, store);

        var selectedId = await selector.SelectAsync(TestContext.CancellationToken);

        selectedId.ShouldBe(3);
        await prompts.Received().AskForPlaylistActionAsync(TestContext.CancellationToken);
        await prompts.Received().AskForNewPlaylistNameAsync(Arg.Is<IReadOnlySet<string>>(names => names.SetEquals(expectedNames)), TestContext.CancellationToken);
        await store.Received().CreateAsync("Study Playlist", TestContext.CancellationToken);
    }
}
