using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spectre.Console;

namespace ShelfMate
{
    public static class FileHandler
    {
        private static readonly string filePath = "bookLibrary.txt";

        // Save a new book to the file
       public static void SaveBook(string[] bookDetails)
{
    try
    {
        if (bookDetails == null || bookDetails.Length < 4)
        {
            AnsiConsole.Markup("[red]Invalid book details provided.[/]\n");
            return;
        }

        if (string.IsNullOrWhiteSpace(bookDetails[0]) || string.IsNullOrWhiteSpace(bookDetails[1]))
        {
            AnsiConsole.Markup("[red]Book title and author are required.[/]\n");
            return;
        }

        File.AppendAllText(filePath, string.Join(",", bookDetails) + Environment.NewLine);
        AnsiConsole.Markup("[green]Book saved successfully![/]\n");
    }
    catch (IOException ioEx)
    {
        AnsiConsole.Markup($"[red]File write error: {ioEx.Message}[/]\n");
    }
    catch (UnauthorizedAccessException uaEx)
    {
        AnsiConsole.Markup($"[red]Access denied: {uaEx.Message}[/]\n");
    }
    catch (Exception ex)
    {
        AnsiConsole.Markup($"[red]Unexpected error saving book: {ex.Message}[/]\n");
    }
}
        // Load books from the file and return them as a list of book details (string arrays)
        public static List<string[]> LoadBooksFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    AnsiConsole.Markup("[yellow]No books found in the library.[/]\n");
                    return new List<string[]>();
                }

                var books = File.ReadAllLines(filePath)
                                .Where(line => !string.IsNullOrWhiteSpace(line)) // Ignore empty lines
                                .Select(line => line.Split(','))
                                .Where(parts => parts.Length >= 4) // Ensure valid format (title, author, genre, status)
                                .ToList();

                return books;
            }
            catch (IOException ioEx)
            {
                AnsiConsole.Markup($"[red]File read error: {ioEx.Message}[/]\n");
            }
            catch (UnauthorizedAccessException uaEx)
            {
                AnsiConsole.Markup($"[red]Access denied: {uaEx.Message}[/]\n");
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Unexpected error loading books: {ex.Message}[/]\n");
            }

            return new List<string[]>(); // Return empty list on failure
        }

        // Save the list of books back to the file
        public static void SaveBooksToFile(List<string[]> books)
        {
            try
            {
                if (books == null || books.Count == 0)
                {
                    AnsiConsole.Markup("[yellow]No books to save.[/]\n");
                    return;
                }

                var lines = books.Select(book => string.Join(",", book));

                File.WriteAllLines(filePath, lines);
                AnsiConsole.Markup("[green]Book library updated successfully![/]\n");
            }
            catch (IOException ioEx)
            {
                AnsiConsole.Markup($"[red]File write error: {ioEx.Message}[/]\n");
            }
            catch (UnauthorizedAccessException uaEx)
            {
                AnsiConsole.Markup($"[red]Access denied: {uaEx.Message}[/]\n");
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Unexpected error saving books: {ex.Message}[/]\n");
            }
        }
    }
}
