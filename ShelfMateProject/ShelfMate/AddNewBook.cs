using System;
using Spectre.Console;

namespace ShelfMate
{
    public class AddNewBook
    {
        public static void Execute()
        {
            AnsiConsole.Write(new FigletText("Add Book").Color(Color.Green));
            Program.PrintRule();

            var bookTitle = AnsiConsole.Ask<string>("Enter the title of the book: ");
            var bookAuthor = AnsiConsole.Ask<string>("Enter the author of the book: ");
            var bookGenre = AnsiConsole.Ask<string>("Enter the genre of the book: ");
            var bookStatus = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What is the status of the book?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(new[] {
                        "Owned", "To Read", "Currently Reading", "Finished"
                    })
            );

            // Prepare the book details
            var newBook = new string[] { bookTitle, bookAuthor, bookGenre, bookStatus, "N/A" }; // Default page number is "N/A"

            try
            {
                // Use FileHandler to save the new book
                FileHandler.SaveBook(newBook);

                AnsiConsole.Markup("[green]Book added successfully![/]\n");
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Error adding book: {ex.Message}[/]\n");
            }
        }
    }
}
