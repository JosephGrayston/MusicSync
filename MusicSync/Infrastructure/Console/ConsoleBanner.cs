using Spectre.Console;

namespace MusicSync.Infrastructure.Console;

public class ConsoleBanner(IAnsiConsole console)
{
    public void DisplayBanner()
    {
        console.WriteLine(
            """
             __  __           _        _____                  
            |  \/  |_   _ ___(_) ___  / ____|_   _ _ __   ___ 
            | |\/| | | | / __| |/ __| \___ \| | | | '_ \ / __|
            | |  | | |_| \__ \ | (__   ___) | |_| | | | | (__ 
            |_|  |_|\__,_|___/_|\___| |____/ \__, |_| |_|\___|
                                             |___/            
            """
        );
    }
}
