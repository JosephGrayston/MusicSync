using MusicSync.Menu;
using Spectre.Console;

namespace MusicSync.Commands
{
    [MenuItem("Compare Playlists")]
    public sealed class ComparePlaylistCommand : IPlaylistCommand
    {
        public async Task ExecuteAsync()
        {
            AnsiConsole.WriteLine("Comparing playlists...");
        }
    }
}
