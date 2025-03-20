// File: Program.cs
using System;
using Spectre.Console;

namespace ShelfMate
{
    class Program
    {
        static void Main(string[] args)
        {
            AnsiConsole.Write(
                new FigletText("ShelfMate")
                    .Color(Color.Cyan1));

            PrintRule();
            Console.WriteLine("\nInitializing the library system...\n");

            while (true) // Keeps looping until the user exits
            {
                var start = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What would you like to do?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                        .AddChoices(new[] {
                            "Add New Book to Library", 
                            "View My Book Library", 
                            "Edit Status of a Book", 
                            "View or Save Page Number", 
                            "Exit"
                        })
                );

                if (start == "Add New Book to Library")
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

                // Pause before returning to the menu
                AnsiConsole.Markup("[blue]\nPress any key to return to the main menu...[/]\n");
                Console.ReadKey(true);
            }
        }

        public static void PrintRule()
        {
            AnsiConsole.Write(new Rule());
        }
    }
}
