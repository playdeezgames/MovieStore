Imports Microsoft.Data.SqlClient
Imports Spectre.Console

Friend Module MainMenu
    Const QuitChoice = "Quit"
    Const MainMenuPrompt = "[olive]Main Menu:[/]"
    Friend Sub Run(connection As SqlConnection)
        Do
            AnsiConsole.Clear()
            Dim prompt As New SelectionPrompt(Of String) With {.Title = "[olive]Main Menu:[/]"}
            prompt.AddChoice(QuitChoice)
            Select Case AnsiConsole.Prompt(prompt)
                Case QuitChoice
                    Exit Do
            End Select
        Loop
    End Sub
End Module
