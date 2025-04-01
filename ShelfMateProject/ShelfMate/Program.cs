using System;
using Spectre.Console;

namespace ShelfMate
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AnsiConsole.Write(
                    new FigletText("ShelfMate")
                        .Color(Color.Cyan1));

                PrintRule();
                Console.WriteLine("\nInitializing the library system...\n");

                while (true) // Keeps looping until the user exits
                {
                    try
                    {
                        var start = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("What would you like to do?")
                                .PageSize(10)
                                .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                                .AddChoices(new[] {
                                    "Search for a Book", 
                                    "Add New Book to Library", 
                                    "View My Book Library", 
                                    "Edit Status of a Book", 
                                    "View or Save Page Number", 
                                    "Exit"
                                })
                        );

                        if (start == "Search for a Book")
                        {
                            BookSearch.Execute(); // Assuming Execute() is the method to trigger search
                        }
                        else if (start == "Add New Book to Library")
                        {
                            AddNewBook.Execute();
                        }
                        else if (start == "View My Book Library")
                        {
                            ViewBookLibrary.Execute();
                        }
                        else if (start == "Edit Status of a Book")
                        {
                            EditBookStatus.Execute();
                        }
                        else if (start == "View or Save Page Number")
                        {
                            ViewOrSavePageNumber.Execute();
                        }
                        else if (start == "Exit")
                        {
                            AnsiConsole.Markup("[yellow]Exiting ShelfMate. Goodbye![/]\n");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.Markup($"[red]An error occurred: {ex.Message}[/]\n");
                        AnsiConsole.Markup("[yellow]Returning to the main menu...[/]\n");
                    }

                    // Pause before returning to the menu
                    AnsiConsole.Markup("[blue]\nPress any key to return to the main menu...[/]\n");
                    Console.ReadKey(true);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Critical error: {ex.Message}[/]\n");
                AnsiConsole.Markup("[yellow]Application is shutting down due to an unexpected error.[/]\n");
            }
        }

        public static void PrintRule()
        {
            try
            {
                AnsiConsole.Write(new Rule());
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Error displaying rule: {ex.Message}[/]\n");
            }
        }
    }
}
