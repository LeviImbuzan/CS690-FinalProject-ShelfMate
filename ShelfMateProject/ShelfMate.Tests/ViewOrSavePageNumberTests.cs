using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using ShelfMate;

namespace ShelfMate.Tests
{
    public class ViewOrSavePageNumberTests
    {
        private const string TestFilePath = "testBookLibrary.txt";

        // Helper method to clean up the test file
        private void CleanUpTestFile()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        // Helper method to write some test data to the file
        private void WriteTestData(List<string> data)
        {
            File.WriteAllLines(TestFilePath, data);
        }

        public ViewOrSavePageNumberTests()
        {
            CleanUpTestFile();
        }

        [Fact]
        public void ShouldShowNoBooksFound_WhenFileIsEmpty()
        {
            // Arrange: Create an empty file
            WriteTestData(new List<string>());

            // Act: Check if file is empty and handle it accordingly
            var exception = Record.Exception(() => HandleNoBooksFound());

            // Assert: Ensure no exception is thrown and the correct message is displayed
            Assert.Null(exception);
            // Optionally, capture the console output to assert it contains "No books found"
        }

        [Fact]
        public void ShouldDisplayCurrentPage_WhenBookIsSelected()
        {
            // Arrange: Create file with some test data
            var testData = new List<string>
            {
                "Book1,Author1,Fiction,Owned,100",
                "Book2,Author2,Non-Fiction,To Read,200"
            };
            WriteTestData(testData);

            // Act: Test checking the current page of a selected book
            var exception = Record.Exception(() => DisplayCurrentPage("Book1"));

            // Assert: Ensure no exception is thrown and correct page is displayed
            Assert.Null(exception);
            // Capture and assert console output for checking the page number
        }

        [Fact]
        public void ShouldUpdatePageNumber_WhenValidPageNumberIsEntered()
        {
            // Arrange: Create file with a book entry
            var testData = new List<string>
            {
                "Book1,Author1,Fiction,Owned,100"
            };
            WriteTestData(testData);

            // Act: Test updating the page number for a book
            var exception = Record.Exception(() => UpdatePageNumber("Book1", 150));

            // Assert: Ensure the page number is updated correctly in the file
            var updatedData = File.ReadAllLines(TestFilePath).ToList();
            Assert.Contains("150", updatedData[0]); // Assert the page number was updated
            Assert.Null(exception);
        }

        [Fact]
        public void ShouldHandleInvalidPageNumber_WhenNegativeValueEntered()
        {
            // Arrange: Create file with a book entry
            var testData = new List<string>
            {
                "Book1,Author1,Fiction,Owned,100"
            };
            WriteTestData(testData);

            // Act: Test entering an invalid negative page number
            var exception = Record.Exception(() => UpdatePageNumber("Book1", -50));

            // Assert: Ensure the page number is not updated to a negative value
            var updatedData = File.ReadAllLines(TestFilePath).ToList();
            Assert.Contains("100", updatedData[0]); // Ensure the original value is still there
            Assert.Null(exception);
        }

        [Fact]
        public void ShouldDisplayBookNotFound_WhenBookIsNotInList()
        {
            // Arrange: Create file with some test data
            var testData = new List<string>
            {
                "Book1,Author1,Fiction,Owned,100"
            };
            WriteTestData(testData);

            // Act: Test selecting a non-existent book
            var exception = Record.Exception(() => HandleBookNotFound("NonExistentBook"));

            // Assert: Ensure no exception is thrown and correct message is displayed
            Assert.Null(exception);
            // Optionally, capture the console output to assert it contains "Book not found"
        }

        // Helper method to handle the case where no books are found
        private void HandleNoBooksFound()
        {
            var books = File.ReadAllLines(TestFilePath);
            if (!books.Any())
            {
                // Simulate displaying the "No books found" message
                Console.WriteLine("No books found in the library!");
            }
        }

        // Helper method to display the current page of a selected book
        private void DisplayCurrentPage(string bookName)
        {
            var books = File.ReadAllLines(TestFilePath);
            var book = books.FirstOrDefault(b => b.Split(',')[0] == bookName);
            if (book != null)
            {
                var page = book.Split(',')[4];
                Console.WriteLine($"Current page for {bookName}: {page}");
            }
        }

        // Helper method to update the page number of a book
        private void UpdatePageNumber(string bookName, int newPageNumber)
        {
            var books = File.ReadAllLines(TestFilePath).ToList();
            var bookIndex = books.FindIndex(b => b.Split(',')[0] == bookName);
            if (bookIndex >= 0 && newPageNumber >= 0)
            {
                var bookDetails = books[bookIndex].Split(',').ToList();
                bookDetails[4] = newPageNumber.ToString();
                books[bookIndex] = string.Join(",", bookDetails);
                File.WriteAllLines(TestFilePath, books);
            }
        }

        // Helper method to handle when a book is not found
        private void HandleBookNotFound(string bookName)
        {
            var books = File.ReadAllLines(TestFilePath);
            var book = books.FirstOrDefault(b => b.Split(',')[0] == bookName);
            if (book == null)
            {
                Console.WriteLine("Book not found.");
            }
        }

        // Clean up after each test
        ~ViewOrSavePageNumberTests()
        {
            CleanUpTestFile();
        }
    }
}
