using Spectre.Console;
using System.Data;

namespace MovieStore
{
    internal static class MainMenu
    {
        const string SearchForTitle = "Search for title...";
        const string Quit = "Quit";
        internal static void Run(IDbConnection connection)
        {
            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[olive]Main Menu:[/]");
                AnsiConsole.MarkupLine($"Titles: {Titles.Count(connection)}");
                var prompt = new SelectionPrompt<string>() { Title=string.Empty };
                prompt.AddChoice(SearchForTitle);
                prompt.AddChoice(Quit);
                switch(AnsiConsole.Prompt(prompt))
                {
                    case SearchForTitle:
                        TitleSearch.Run(connection); 
                        break;
                    case Quit:
                        return;
                }
            }
        }
    }
}
