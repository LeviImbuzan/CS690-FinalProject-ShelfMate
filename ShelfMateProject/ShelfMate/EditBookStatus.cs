using System;
using System.IO;
using System.Linq;
using Spectre.Console;

namespace ShelfMate
{
    public class EditBookStatus
    {
        public static void Execute()
        {
            AnsiConsole.Write(new FigletText("Book Status Update").Color(Color.Green));
            Program.PrintRule();

            if (!File.Exists("bookLibrary.txt"))
            {
                AnsiConsole.Markup("[red]No books found in the library.[/]\n");
                return;
            }

            // Use FileHandler to read books from the file
            var books = FileHandler.LoadBooksFromFile();

            if (books.Count == 0)
            {
                AnsiConsole.Markup("[yellow]No books available to update.[/]\n");
                return;
            }

            // Let the user select a book to edit
            var selectedBook = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a book to update its status:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(books.Select(book => $"{book[0]} by {book[1]} ({book[3]})").ToList())
            );

            // Extract book details
            var bookIndex = books.FindIndex(book => $"{book[0]} by {book[1]} ({book[3]})" == selectedBook);
            if (bookIndex == -1) return; // Just in case

            // Let the user select a new status
            var newStatus = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Change status of \"{books[bookIndex][0]}\" to:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(new[] {
                        "Owned", "To Read", "Currently Reading", "Finished"
                    })
            );

            // Update the status
            books[bookIndex][3] = newStatus;

            // Use FileHandler to save the updated book list
            FileHandler.SaveBooksToFile(books);

            AnsiConsole.Markup($"[green]Status updated successfully![/]\n");
        }
    }
}
