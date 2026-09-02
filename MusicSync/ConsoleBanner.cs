using Spectre.Console;

namespace MusicSync
{
    public static class ConsoleBanner
    {
        public static void DisplayBanner()
        {
            AnsiConsole.WriteLine(
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
}
