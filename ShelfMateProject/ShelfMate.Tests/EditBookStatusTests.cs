using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using ShelfMate;

namespace ShelfMate.Tests
{
    public class EditBookStatusTests
    {
        [Fact]
        public void BookIndex_ShouldBe_CorrectlyIdentified()
        {
            // Arrange
            var books = new List<string[]>
            {
                new string[] { "The Hobbit", "J.R.R. Tolkien", "Fantasy", "Owned", "N/A" },
                new string[] { "1984", "George Orwell", "Dystopian", "To Read", "N/A" }
            };
            var selectedBook = "1984 by George Orwell (To Read)";

            // Act
            var bookIndex = books.FindIndex(book => $"{book[0]} by {book[1]} ({book[3]})" == selectedBook);

            // Assert
            Assert.Equal(1, bookIndex);
        }

        [Fact]
        public void BookChoices_ShouldContain_CorrectlyFormattedEntries()
        {
            // Arrange
            var books = new List<string[]>
            {
                new string[] { "The Hobbit", "J.R.R. Tolkien", "Fantasy", "Owned", "N/A" },
                new string[] { "1984", "George Orwell", "Dystopian", "To Read", "N/A" }
            };

            // Act
            var bookChoices = books.Select(book => $"{book[0]} by {book[1]} ({book[3]})").ToList();

            // Assert
            Assert.Equal(2, bookChoices.Count);
            Assert.Contains("The Hobbit by J.R.R. Tolkien (Owned)", bookChoices);
            Assert.Contains("1984 by George Orwell (To Read)", bookChoices);
        }
    }
}
