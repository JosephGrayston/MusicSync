using MusicSync.Menu;
using Spectre.Console;

namespace MusicSync.Commands
{
    [MenuItem("Fetch Destination Playlist Data")]
    public sealed class FetchDestinationPlaylistCommand : IPlaylistCommand
    {
        public async Task ExecuteAsync()
        {
            AnsiConsole.WriteLine("Fetching destination playlist data...");
        }
    }
}
