using System;
using System.IO;
using System.Linq;
using Spectre.Console;

namespace ShelfMate
{
    public class ViewOrSavePageNumber
    {
        public static void Execute()
        {
            AnsiConsole.Write(new FigletText("Page Number").Color(Color.Green));
            Program.PrintRule();

            var books = File.ReadAllLines("bookLibrary.txt").ToList();

            if (books.Count == 0)
            {
                AnsiConsole.Markup("[red]No books found in the library![/]\n");
                return;
            }

            // User selects a book (assuming title is at index 0)
            var selectedBook = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a book:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(books.Select(line => line.Split(',')[0]))
            );

            var bookIndex = books.FindIndex(line => line.StartsWith(selectedBook + ","));
            if (bookIndex == -1)
            {
                AnsiConsole.Markup("[red]Book not found![/]\n");
                return;
            }

            var bookDetails = books[bookIndex].Split(',').ToList();
            int pageNumberIndex = 4; // Assuming page number is stored at index 4

            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"What would you like to do with [green]{selectedBook}[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(new[] { "Check Current Page Number", "Save New Page Number" })
            );

            if (action == "Check Current Page Number")
            {
                string currentPage = bookDetails.Count > pageNumberIndex ? bookDetails[pageNumberIndex] : "N/A";
                AnsiConsole.Markup($"[green]{selectedBook}[/] is currently on page: [cyan]{currentPage}[/]\n");
            }
            else if (action == "Save New Page Number")
            {
                int newPage = AnsiConsole.Ask<int>("Enter the new page number:");

                // Ensure we have enough elements in the list
                while (bookDetails.Count <= pageNumberIndex)
                {
                    bookDetails.Add("N/A");
                }

                // Update the page number in the book details
                bookDetails[pageNumberIndex] = newPage.ToString();
                books[bookIndex] = string.Join(",", bookDetails);

                // Write updated books back to the file
                File.WriteAllLines("bookLibrary.txt", books);

                AnsiConsole.Markup($"[green]{selectedBook}[/] page number updated to: [cyan]{newPage}[/]\n");
            }
        }
    }
}
