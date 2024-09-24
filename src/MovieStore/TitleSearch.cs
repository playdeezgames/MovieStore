using Spectre.Console;
using System.Data;

namespace MovieStore
{
    internal static class TitleSearch
    {   
        const string Cancel = "Cancel";

        internal static void Run(IDbConnection connection)
        {
            var filter = AnsiConsole.Ask<string>("[olive]Filter:[/]");
            if (string.IsNullOrWhiteSpace(filter))
            {
                filter = "%";
            }
            else
            {
                filter = $"%{filter}%";
            }
            AnsiConsole.Clear();
            var table = Titles.Filter(connection, filter).ToDictionary(x=>$"{x.TitleName}{(x.TitleYear.HasValue?$"({x.TitleYear.Value})":String.Empty)}(#{x.TitleId})",x=>x.TitleId);
            var prompt = new SelectionPrompt<string> { Title = $"[olive]Titles filtered by '{filter}':[/]" };
            prompt.AddChoice(Cancel);
            prompt.AddChoices(table.Keys);
            var answer = AnsiConsole.Prompt(prompt);
            if (answer == Cancel)
            {
                return;
            }
            var titleId = table[answer];
            TitleView.Run(connection, titleId);
        }
    }
}