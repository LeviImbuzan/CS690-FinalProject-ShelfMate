using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spectre.Console;

namespace ShelfMate
{
    public class BookSearch
    {
        private List<Book> books;
        private string filePath = "bookLibrary.txt";

        public BookSearch()
        {
            books = LoadBooksFromFile();
        }

        private List<Book> LoadBooksFromFile()
        {
            List<Book> bookList = new List<Book>();

            if (!File.Exists(filePath))
            {
                AnsiConsole.MarkupLine("[red]The book library file does not exist.[/]\n");
                return bookList;
            }

            try
            {
                var lines = File.ReadAllLines(filePath);
                if (lines.Length == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]The book library file is empty.[/]\n");
                }

                foreach (var line in lines)
                {
                    var parts = line.Split(',');

                    if (parts.Length < 2) // If the line does not contain at least title and author
                    {
                        AnsiConsole.MarkupLine($"[yellow]Skipping malformed line: {line}[/]\n");
                        continue; // Skip malformed line
                    }

                    bookList.Add(new Book
                    {
                        Title = parts[0].Trim(),
                        Author = parts[1].Trim(),
                        Genre = parts.Length > 2 ? parts[2].Trim() : "Unknown",
                        Status = parts.Length > 3 ? parts[3].Trim() : "Unknown",
                        PageCount = parts.Length > 4 ? parts[4].Trim() : "N/A"
                    });
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]An error occurred while reading the file: {ex.Message}[/]\n");
            }

            return bookList;
        }

        public List<Book> SearchByTitle(string title)
        {
            title = NormalizeInput(title);
            return books.Where(b => NormalizeInput(b.Title).Contains(title)).ToList();
        }

        public List<Book> SearchByAuthor(string author)
        {
            author = NormalizeInput(author);
            return books.Where(b => NormalizeInput(b.Author).Contains(author)).ToList();
        }

        private string NormalizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be empty or whitespace.");
            }

            // Sanitize input to remove any special characters that might break searches
            return input.Trim().ToLower();
        }

        public void DisplaySearchResults(List<Book> results)
        {
            if (results.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No books found.[/]\n");
            }
            else
            {
                var table = new Table();
                table.AddColumn("[bold]Title[/]");
                table.AddColumn("[bold]Author[/]");
                table.AddColumn("[bold]Genre[/]");
                table.AddColumn("[bold]Status[/]");
                table.AddColumn("[bold]Page Count[/]");

                foreach (var book in results)
                {
                    table.AddRow(book.Title, book.Author, book.Genre, book.Status, book.PageCount);
                }

                AnsiConsole.Write(table);
            }
        }

        public static void Execute()
        {
            var bookSearch = new BookSearch();

            while (true)
            {
                try
                {
                    var searchOption = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[green]Search by:[/]")
                            .AddChoices("Title", "Author"));

                    string query = AnsiConsole.Ask<string>("[blue]Enter search term:[/]");

                    if (string.IsNullOrWhiteSpace(query))
                    {
                        AnsiConsole.MarkupLine("[red]Search term cannot be empty or whitespace.[/]\n");
                        continue; // Prompt the user again
                    }

                    List<Book> results = searchOption == "Title" ? bookSearch.SearchByTitle(query) : bookSearch.SearchByAuthor(query);
                    bookSearch.DisplaySearchResults(results);

                    var nextAction = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[green]What would you like to do next?[/]")
                            .AddChoices("Search Again", "Return to Main Menu"));

                    if (nextAction == "Return to Main Menu")
                    {
                        break;
                    }
                }
                catch (ArgumentException ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]\n");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]An unexpected error occurred: {ex.Message}[/]\n");
                }
            }
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Status { get; set; }
        public string PageCount { get; set; }
    }
}
