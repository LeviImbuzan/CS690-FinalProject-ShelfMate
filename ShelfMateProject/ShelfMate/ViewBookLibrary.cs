using System;
using System.IO;
using System.Linq;
using Spectre.Console;

namespace ShelfMate
{
    public class ViewBookLibrary
    {
        public static void Execute()
        {
            AnsiConsole.Write(new FigletText("My Book Library").Color(Color.Green));
            Program.PrintRule();

            var bookLibrary = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a category to view books:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(new[] {
                        "View All Books", "View Owned Books", "View To Read Books", "View Currently Reading Books", "View Finished Books"
                    })
            );

            string selectedStatus = bookLibrary switch
            {
                "View Owned Books" => "Owned",
                "View To Read Books" => "To Read",
                "View Currently Reading Books" => "Currently Reading",
                "View Finished Books" => "Finished",
                _ => null
            };

            if (selectedStatus != null)
            {
                DisplayBooksByStatus(selectedStatus);
            }
            else
            {
                DisplayAllBooks();
            }
        }

        private static void DisplayBooksByStatus(string status)
        {
            if (!File.Exists("bookLibrary.txt"))
            {
                AnsiConsole.Markup("[red]No books found in the library.[/]\n");
                return;
            }

            var books = File.ReadAllLines("bookLibrary.txt")
                            .Where(line => !string.IsNullOrWhiteSpace(line)) // Ignore empty lines
                            .Select(line => line.Split(','))
                            .Where(parts => parts.Length >= 4) // Ensure there are at least 4 parts (title, author, genre, status)
                            .ToList();

            if (books.Count == 0)
            {
                AnsiConsole.Markup($"[yellow]No books found with status: {status}[/]\n");
                return;
            }

            var table = new Table();
            table.AddColumn("Title");
            table.AddColumn("Author");
            table.AddColumn("Genre");
            table.AddColumn("Status");
            table.AddColumn("Page Number");

            table.ShowRowSeparators();

            foreach (var book in books)
            {
                string pageNumber = book.Length >= 5 ? book[4] : "N/A"; // Check if page number exists (at index 4)
                table.AddRow(book[0], book[1], book[2], book[3], pageNumber);
            }

            AnsiConsole.Write(table);
        }

        private static void DisplayAllBooks()
        {
            if (!File.Exists("bookLibrary.txt"))
            {
                AnsiConsole.Markup("[red]No books found in the library.[/]\n");
                return;
            }

            var books = File.ReadAllLines("bookLibrary.txt")
                            .Where(line => !string.IsNullOrWhiteSpace(line)) // Ignore empty lines
                            .Select(line => line.Split(','))
                            .Where(parts => parts.Length >= 4) // Ensure there are at least 4 parts (title, author, genre, status)
                            .ToList();

            if (books.Count == 0)
            {
                AnsiConsole.Markup("[yellow]No books found in the library.[/]\n");
                return;
            }

            var table = new Table();
            table.AddColumn("Title");
            table.AddColumn("Author");
            table.AddColumn("Genre");
            table.AddColumn("Status");
            table.AddColumn("Page Number");

            table.ShowRowSeparators();

            foreach (var book in books)
            {
                string pageNumber = book.Length >= 5 ? book[4] : "N/A"; // Check if page number exists (at index 4)
                table.AddRow(book[0], book[1], book[2], book[3], pageNumber);
            }

            AnsiConsole.Write(table);
        }
    }
}
