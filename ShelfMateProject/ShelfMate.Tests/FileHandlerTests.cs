using System;
using System.Collections.Generic;
using Xunit;
using ShelfMate;

namespace ShelfMate.Tests
{
    public class FileHandlerTests
    {
        [Fact]
        public void SaveBook_ShouldSaveBookSuccessfully_WhenValidDataProvided()
        {
            // Arrange
            var bookDetails = new string[] { "Book1", "Author1", "Fiction", "Owned" };

            // Act
            FileHandler.SaveBook(bookDetails);

            // Assert
            var books = FileHandler.LoadBooksFromFile();
            Assert.Contains(books, book => book[0] == "Book1" && book[1] == "Author1");
        }

[Fact]
public void SaveBook_ShouldNotSave_WhenInvalidDataProvided()
{
    // Arrange
    var invalidBookDetails = new string[] { "Book1", "Author1" }; // Missing genre and status

    // Act
    FileHandler.SaveBook(invalidBookDetails);

    // Assert
    var books = FileHandler.LoadBooksFromFile();
    Assert.DoesNotContain(books, book => book[0] == "Book1" && book[1] == "Author1");
}

        [Fact]
        public void LoadBooksFromFile_ShouldReturnBooks_WhenFileContainsValidData()
        {
            // Arrange
            var expectedBooks = new List<string[]>
            {
                new string[] { "Book1", "Author1", "Fiction", "Owned" },
                new string[] { "Book2", "Author2", "Non-Fiction", "To Read" }
            };
            
            // Simulating saving books to a file for testing
            FileHandler.SaveBooksToFile(expectedBooks);

            // Act
            var books = FileHandler.LoadBooksFromFile();

            // Assert
            Assert.Equal(expectedBooks.Count, books.Count);
            Assert.Equal(expectedBooks[0][0], books[0][0]);
            Assert.Equal(expectedBooks[1][0], books[1][0]);
        }

        [Fact]
        public void SaveBooksToFile_ShouldUpdateBooksList_WhenValidBooksProvided()
        {
            // Arrange
            var updatedBooks = new List<string[]>
            {
                new string[] { "UpdatedBook1", "UpdatedAuthor1", "Drama", "Owned" },
                new string[] { "UpdatedBook2", "UpdatedAuthor2", "Biography", "To Read" }
            };

            // Act
            FileHandler.SaveBooksToFile(updatedBooks);

            // Assert
            var books = FileHandler.LoadBooksFromFile();
            Assert.Equal(updatedBooks.Count, books.Count);
            Assert.Equal(updatedBooks[0][0], books[0][0]);
            Assert.Equal(updatedBooks[1][0], books[1][0]);
        }
    }
}
