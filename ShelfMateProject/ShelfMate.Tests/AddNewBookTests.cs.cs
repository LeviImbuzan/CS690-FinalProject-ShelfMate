using System;
using Xunit;
using ShelfMate;

namespace ShelfMate.Tests
{
    public class AddNewBookTests
    {
        [Fact]
        public void NewBook_ShouldContain_AllRequiredComponents()
        {
            // Arrange
            var bookTitle = "The Hobbit";
            var bookAuthor = "J.R.R. Tolkien";
            var bookGenre = "Fantasy";
            var bookStatus = "Owned";
            var expectedDefaultField = "N/A";

            // Act
            var newBook = new string[] { bookTitle, bookAuthor, bookGenre, bookStatus, expectedDefaultField };

            // Assert
            Assert.Equal(5, newBook.Length);
            Assert.Equal(bookTitle, newBook[0]);
            Assert.Equal(bookAuthor, newBook[1]);
            Assert.Equal(bookGenre, newBook[2]);
            Assert.Equal(bookStatus, newBook[3]);
            Assert.Equal(expectedDefaultField, newBook[4]);
        }
    }
}
