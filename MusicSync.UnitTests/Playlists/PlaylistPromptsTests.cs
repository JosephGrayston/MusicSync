using Microsoft.Extensions.Time.Testing;
using MusicSync.Infrastructure.Terminal;
using MusicSync.Playlists;
using Shouldly;
using Spectre.Console.Testing;

namespace MusicSync.UnitTests.Playlists;

[TestClass]
public sealed class PlaylistPromptsTests
{
    private static readonly DateTimeOffset _now = new(2026, 11, 23, 9, 30, 0, TimeSpan.Zero);

    public TestContext TestContext { get; set; }

    [TestMethod]
    public async Task AskForPlaylistActionAsyncFirstOptionIsSelectExistingPlaylist()
    {
        var (prompts, console) = Create();

        console.Input.PushKey(ConsoleKey.Enter);

        var action = await prompts.AskForPlaylistActionAsync(TestContext.CancellationToken);

        action.ShouldBe(PlaylistAction.SelectExistingPlaylist);
    }

    [TestMethod]
    public async Task AskForPlaylistActionAsyncSecondOptionIsCreateNewPlaylist()
    {
        var (prompts, console) = Create();

        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Enter);

        var action = await prompts.AskForPlaylistActionAsync(TestContext.CancellationToken);

        action.ShouldBe(PlaylistAction.CreateNewPlaylist);
    }

    [TestMethod]
    public async Task ChoosePlaylistAsyncWithPlaylistIncludingBracketsDoesNotThrow()
    {
        var (prompts, console) = Create();

        var playlists = new[]
        {
            new Playlist { Id = 1, Name = "Chill", CreatedAt = DateTimeOffset.UnixEpoch },
            new Playlist { Id = 2, Name = "Rock [Live]", CreatedAt = DateTimeOffset.UnixEpoch }
        };

        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Enter);

        var chosen = await prompts.ChoosePlaylistAsync(playlists, TestContext.CancellationToken);

        chosen.Id.ShouldBe(2);
        console.Output.ShouldContain("Rock [Live]");
    }

    [TestMethod]
    public async Task ChoosePlaylistAsyncAllowsSearching()
    {
        var (prompts, console) = Create();

        var playlists = new[]
        {
            new Playlist { Id = 1, Name = "Chill", CreatedAt = DateTimeOffset.UnixEpoch },
            new Playlist { Id = 2, Name = "Rock [Live]", CreatedAt = DateTimeOffset.UnixEpoch },
            new Playlist { Id = 2, Name = "Jazz", CreatedAt = DateTimeOffset.UnixEpoch }
        };

        console.Input.PushTextWithEnter("Rock");

        var chosen = await prompts.ChoosePlaylistAsync(playlists, TestContext.CancellationToken);
        chosen.Id.ShouldBe(2);
    }

    [TestMethod]
    public async Task ChoosePlaylistAsyncDisplaysPlayistsNameAndCreatedAt()
    {
        var formatter = new DateTimeFormatter(new FakeTimeProvider());
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        var prompts = new PlaylistPrompts(console, formatter);

        var playlist = new Playlist { Id = 2, Name = "Rock [Live]", CreatedAt = _now };

        console.Input.PushTextWithEnter("Rock");

        await prompts.ChoosePlaylistAsync([playlist], TestContext.CancellationToken);

        console.Output.ShouldContain($"Rock [Live] (Created: {formatter.ToDisplay(_now)})");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("    ")]
    public async Task AskForNewPlaylistNameAsyncEmptyNameShowsErrorThenAcceptsValidName(string playlistName)
    {
        var (prompts, console) = Create();

        var playlists = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Rock [Live]" };

        console.Input.PushTextWithEnter(playlistName);
        console.Input.PushTextWithEnter("Fresh");

        var name = await prompts.AskForNewPlaylistNameAsync(playlists, TestContext.CancellationToken);

        name.ShouldBe("Fresh");
        console.Output.ShouldContain("Playlist name cannot be empty.");
    }

    [TestMethod]
    public async Task AskForNewPlaylistNameAsyncTooLongShowsErrorThenAcceptsValidName()
    {
        var (prompts, console) = Create();

        var playlists = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Rock [Live]" };

        console.Input.PushTextWithEnter(new string('A', 201));
        console.Input.PushTextWithEnter("Fresh");

        var name = await prompts.AskForNewPlaylistNameAsync(playlists, TestContext.CancellationToken);

        name.ShouldBe("Fresh");
        console.Output.ShouldContain($"Playlist name is too long. Cannot exceed {PlaylistNameValidator.MaxLength} characters.");
    }

    [TestMethod]
    [DataRow("Rock [Live]")]
    [DataRow("rock [live]")]
    [DataRow("ROCK [LIVE]")]
    [DataRow("    rock [live]    ")]
    public async Task AskForNewPlaylistNameAsyncDuplicateNameShowsErrorThenAcceptsValidName(string playlistName)
    {
        var (prompts, console) = Create();

        var playlists = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Rock [Live]" };

        console.Input.PushTextWithEnter(playlistName);
        console.Input.PushTextWithEnter("Fresh");

        var name = await prompts.AskForNewPlaylistNameAsync(playlists, TestContext.CancellationToken);

        name.ShouldBe("Fresh");
        console.Output.ShouldContain("A playlist with that name already exists.");
    }

    [TestMethod]
    [DataRow("Hip Hop", "Hip Hop")]
    [DataRow("   R&B", "R&B")]
    [DataRow("Smooth   ", "Smooth")]
    [DataRow("    Chilled     ", "Chilled")]
    public async Task AskForNewPlaylistNameAsyncValidNameReturnsTrimmedName(string playlistName, string expectedResult)
    {
        var (prompts, console) = Create();

        var playlists = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Rock [Live]" };

        console.Input.PushTextWithEnter(playlistName);

        var name = await prompts.AskForNewPlaylistNameAsync(playlists, TestContext.CancellationToken);

        name.ShouldBe(expectedResult);
    }

    private static (PlaylistPrompts, TestConsole) Create()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        var formatter = new DateTimeFormatter(new FakeTimeProvider());

        return (new PlaylistPrompts(console, formatter), console);
    }
}
