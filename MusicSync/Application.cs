using MusicSync.Commands;
using MusicSync.Menu;
using Spectre.Console;
using System.Reflection;

namespace MusicSync
{
    public sealed class Application(IEnumerable<IPlaylistCommand> commands)
    {
        public async Task<int> RunAsync()
        {
            ConsoleBanner.DisplayBanner();

            var menuOptions = commands
                .Select(command => new MenuOption(
                    GetMenuName(command),
                    command))
                .Append(new MenuOption("Exit", null));

            var selectedOption = await AnsiConsole.PromptAsync(
                new SelectionPrompt<MenuOption>()
                    .Title("Please select an operation")
                    .AddChoices(menuOptions)
                    .UseConverter(option => option.Description));
            
            if (selectedOption.Command is not null)
            {
                await selectedOption.Command.ExecuteAsync();
            }


          //1. Fetch current YouTube playlist
          //2. Compare playlists
          //3. Find YouTube matches  - TO DO add command for this and review steps
          //4. Review matches
          //5. Publish playlist
          //6. Exit

            return 0;
        }

        private static string GetMenuName(IPlaylistCommand command)
        {
           var attribute = command.GetType().GetCustomAttribute<MenuItemAttribute>();

            return attribute?.Description ?? command.GetType().Name;
        }
    }
}
