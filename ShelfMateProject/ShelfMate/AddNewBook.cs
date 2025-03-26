using System;
using Spectre.Console;

namespace ShelfMate
{
    public class AddNewBook
    {
        public static void Execute()
        {
            try
            {
                AnsiConsole.Write(new FigletText("Add Book").Color(Color.Green));
                Program.PrintRule();

                var bookTitle = PromptNonEmptyString("Enter the title of the book: ");
                var bookAuthor = PromptNonEmptyString("Enter the author of the book: ");
                var bookGenre = PromptNonEmptyString("Enter the genre of the book: ");
                var bookStatus = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What is the status of the book?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                        .AddChoices(new[] { "Owned", "To Read", "Currently Reading", "Finished" })
                );

                var newBook = new string[] { bookTitle, bookAuthor, bookGenre, bookStatus, "N/A" };

                try
                {
                    FileHandler.SaveBook(newBook);
                    AnsiConsole.Markup("[green]Book added successfully![/]\n");
                }
                catch (IOException ioEx)
                {
                    AnsiConsole.Markup($"[red]File error: {ioEx.Message}[/]\n");
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    AnsiConsole.Markup($"[red]Access error: {uaEx.Message}[/]\n");
                }
                catch (Exception ex)
                {
                    AnsiConsole.Markup($"[red]Unexpected error while saving the book: {ex.Message}[/]\n");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]An unexpected error occurred: {ex.Message}[/]\n");
            }
        }

        private static string PromptNonEmptyString(string prompt)
        {
            string input;
            do
            {
                input = AnsiConsole.Ask<string>(prompt).Trim();
                if (string.IsNullOrEmpty(input))
                {
                    AnsiConsole.Markup("[red]Input cannot be empty. Please try again.[/]\n");
                }
            } while (string.IsNullOrEmpty(input));

            return input;
        }
    }
}
