using System;
using System.Collections.Generic;
using System.IO;

namespace ShelfMate
{
    public static class FileHandler
    {
        private static string filePath = "bookLibrary.txt";

        // Save a new book to the file
        public static void SaveBook(string[] bookDetails)
        {
            // Append the new book details to the file
            File.AppendAllText(filePath, string.Join(",", bookDetails) + Environment.NewLine);
        }

        // Load books from the file and return them as a list of book details (string arrays)
        public static List<string[]> LoadBooksFromFile()
        {
            if (!File.Exists(filePath))
            {
                return new List<string[]>();
            }

            var books = File.ReadAllLines(filePath)
                            .Select(line => line.Split(','))
                            .Where(parts => parts.Length >= 4) // Ensure valid lines (title, author, genre, status)
                            .ToList();

            return books;
        }

        // Save the list of books back to the file
        public static void SaveBooksToFile(List<string[]> books)
        {
            var lines = books.Select(book => string.Join(",", book));
            File.WriteAllLines(filePath, lines);
        }
    }
}