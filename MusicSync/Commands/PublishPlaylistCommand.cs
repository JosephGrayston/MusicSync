using MusicSync.Menu;
using Spectre.Console;

namespace MusicSync.Commands
{
    [MenuItem("Publish Playlist")]
    public sealed class PublishPlaylistCommand : IPlaylistCommand
    {
        public async Task ExecuteAsync()
        {
            AnsiConsole.WriteLine("Publishing playlist...");
        }
    }
}
