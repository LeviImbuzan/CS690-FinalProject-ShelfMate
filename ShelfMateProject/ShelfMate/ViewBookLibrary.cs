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
            try
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
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]An unexpected error occurred: {ex.Message}[/]\n");
            }
        }

        private static void DisplayBooksByStatus(string status)
        {
            try
            {
                var books = LoadBooks();
                var filteredBooks = books.Where(book => book.Length >= 4 && book[3] == status).ToList();

                if (filteredBooks.Count == 0)
                {
                    AnsiConsole.Markup($"[yellow]No books found with status: {status}[/]\n");
                    return;
                }

                DisplayBookTable(filteredBooks);
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Error displaying books: {ex.Message}[/]\n");
            }
        }

        private static void DisplayAllBooks()
        {
            try
            {
                var books = LoadBooks();

                if (books.Count == 0)
                {
                    AnsiConsole.Markup("[yellow]No books found in the library.[/]\n");
                    return;
                }

                DisplayBookTable(books);
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Error displaying books: {ex.Message}[/]\n");
            }
        }

        private static List<string[]> LoadBooks()
        {
            if (!File.Exists("bookLibrary.txt"))
            {
                throw new FileNotFoundException("No books found in the library.");
            }

            try
            {
                return File.ReadAllLines("bookLibrary.txt")
                           .Where(line => !string.IsNullOrWhiteSpace(line)) // Ignore empty lines
                           .Select(line => line.Split(','))
                           .Where(parts => parts.Length >= 4) // Ensure there are at least 4 parts (title, author, genre, status)
                           .ToList();
            }
            catch (IOException ioEx)
            {
                throw new IOException($"File error: {ioEx.Message}");
            }
            catch (UnauthorizedAccessException uaEx)
            {
                throw new UnauthorizedAccessException($"Access error: {uaEx.Message}");
            }
        }

        private static void DisplayBookTable(List<string[]> books)
        {
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
