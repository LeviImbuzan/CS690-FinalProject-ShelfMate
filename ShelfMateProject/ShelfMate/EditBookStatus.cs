using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Spectre.Console;

namespace ShelfMate
{
    public class EditBookStatus
    {
        public static void Execute()
        {
            try
            {
                AnsiConsole.Write(new FigletText("Book Status Update").Color(Color.Green));
                Program.PrintRule();

                if (!File.Exists("bookLibrary.txt"))
                {
                    AnsiConsole.Markup("[red]No books found in the library.[/]\n");
                    return;
                }

                // Attempt to load books
                List<string[]> books;
                try
                {
                    books = FileHandler.LoadBooksFromFile();
                }
                catch (IOException ioEx)
                {
                    AnsiConsole.Markup($"[red]File error: {ioEx.Message}[/]\n");
                    return;
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    AnsiConsole.Markup($"[red]Access denied: {uaEx.Message}[/]\n");
                    return;
                }
                catch (Exception ex)
                {
                    AnsiConsole.Markup($"[red]Unexpected error loading books: {ex.Message}[/]\n");
                    return;
                }

                if (books.Count == 0)
                {
                    AnsiConsole.Markup("[yellow]No books available to update.[/]\n");
                    return;
                }

                // Let the user select a book to edit
                var bookChoices = books.Select(book => $"{book[0]} by {book[1]} ({book[3]})").ToList();
                
                if (bookChoices.Count == 0)
                {
                    AnsiConsole.Markup("[yellow]No valid books found to edit.[/]\n");
                    return;
                }

                string selectedBook;
                try
                {
                    selectedBook = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Select a book to update its status:")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                            .AddChoices(bookChoices)
                    );
                }
                catch (Exception ex)
                {
                    AnsiConsole.Markup($"[red]Error selecting a book: {ex.Message}[/]\n");
                    return;
                }

                // Extract book details
                var bookIndex = books.FindIndex(book => $"{book[0]} by {book[1]} ({book[3]})" == selectedBook);
                if (bookIndex == -1)
                {
                    AnsiConsole.Markup("[red]Selected book could not be found.[/]\n");
                    return;
                }

                // Let the user select a new status
                string newStatus;
                try
                {
                    newStatus = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title($"Change status of \"{books[bookIndex][0]}\" to:")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                            .AddChoices(new[] {
                                "Owned", "To Read", "Currently Reading", "Finished"
                            })
                    );
                }
                catch (Exception ex)
                {
                    AnsiConsole.Markup($"[red]Error selecting a new status: {ex.Message}[/]\n");
                    return;
                }

                // Update the status
                books[bookIndex][3] = newStatus;

                // Attempt to save updated book list
                try
                {
                    FileHandler.SaveBooksToFile(books);
                    AnsiConsole.Markup($"[green]Status updated successfully![/]\n");
                }
                catch (IOException ioEx)
                {
                    AnsiConsole.Markup($"[red]File write error: {ioEx.Message}[/]\n");
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    AnsiConsole.Markup($"[red]Access denied while saving: {uaEx.Message}[/]\n");
                }
                catch (Exception ex)
                {
                    AnsiConsole.Markup($"[red]Unexpected error saving books: {ex.Message}[/]\n");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]An unexpected error occurred: {ex.Message}[/]\n");
            }
        }
    }
}
