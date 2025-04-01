using System;
using System.IO;
using ShelfMate;
using Xunit;

namespace ShelfMate.Tests
{
    public class BookSearchTests
    {
        // Test SearchByTitle Method
        [Fact]
        public void SearchByTitle_ReturnsBooks_WhenTitleMatches()
        {
            // Arrange - Ensure the test file exists and contains expected data
            PrepareTestFile();

            var bookSearch = new BookSearch();

            // Act
            var result = bookSearch.SearchByTitle("Blood and Oil");

            // Assert
            Assert.Single(result);
            Assert.Equal("Blood and Oil", result[0].Title);
            Assert.Equal("Bradley Hope", result[0].Author);
        }

        [Fact]
        public void SearchByTitle_ReturnsEmptyList_WhenNoTitleMatches()
        {
            // Arrange - Ensure the test file exists and contains expected data
            PrepareTestFile();

            var bookSearch = new BookSearch();

            // Act
            var result = bookSearch.SearchByTitle("Nonexistent Book");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void SearchByTitle_HandlesCaseInsensitiveSearch()
        {
            // Arrange - Ensure the test file exists and contains expected data
            PrepareTestFile();

            var bookSearch = new BookSearch();

            // Act
            var result = bookSearch.SearchByTitle("blood and oil");

            // Assert
            Assert.Single(result);
            Assert.Equal("Blood and Oil", result[0].Title);
        }

        [Fact]
        public void SearchByTitle_HandlesPartialTitleMatch()
        {
            // Arrange - Ensure the test file exists and contains expected data
            PrepareTestFile();

            var bookSearch = new BookSearch();

            // Act
            var result = bookSearch.SearchByTitle("Blood");

            // Assert
            Assert.Single(result);
            Assert.Equal("Blood and Oil", result[0].Title);
        }

        // Test SearchByAuthor Method
        [Fact]
        public void SearchByAuthor_ReturnsBooks_WhenAuthorMatches()
        {
            // Arrange - Ensure the test file exists and contains expected data
            PrepareTestFile();

            var bookSearch = new BookSearch();

            // Act
            var result = bookSearch.SearchByAuthor("Bradley Hope");

            // Assert
            Assert.Single(result);
            Assert.Equal("Blood and Oil", result[0].Title);
            Assert.Equal("Bradley Hope", result[0].Author);
        }

        [Fact]
        public void SearchByAuthor_ReturnsEmptyList_WhenNoAuthorMatches()
        {
            // Arrange - Ensure the test file exists and contains expected data
            PrepareTestFile();

            var bookSearch = new BookSearch();

            // Act
            var result = bookSearch.SearchByAuthor("Nonexistent Author");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void SearchByAuthor_HandlesCaseInsensitiveSearch()
        {
            // Arrange - Ensure the test file exists and contains expected data
            PrepareTestFile();

            var bookSearch = new BookSearch();

            // Act
            var result = bookSearch.SearchByAuthor("dmitry glukhovsky");

            // Assert
            Assert.Single(result);
            Assert.Equal("Metro 2033", result[0].Title);
            Assert.Equal("Dmitry Glukhovsky", result[0].Author);
        }

        [Fact]
        public void SearchByAuthor_HandlesPartialAuthorMatch()
        {
            // Arrange - Ensure the test file exists and contains expected data
            PrepareTestFile();

            var bookSearch = new BookSearch();

            // Act
            var result = bookSearch.SearchByAuthor("Tolkien");

            // Assert
            Assert.Single(result);
            Assert.Equal("The Hobbit", result[0].Title);
            Assert.Equal("J.R.R. Tolkien", result[0].Author);
        }

        // Helper method to prepare the test file with known data
        private void PrepareTestFile()
        {
            var testData = "Blood and Oil,Bradley Hope,Biography,Finished,850\n" +
                           "Metro 2033,Dmitry Glukhovsky,Horror,Owned,N/A\n" +
                           "Cryptonomicon,Neal Stephenson,Drama,To Read,6255\n" +
                           "The Hobbit,J.R.R. Tolkien,Fantasy,Owned,N/A\n" +
                           "1984,George Orwell,Dystopian,To Read,N/A";

            File.WriteAllText("bookLibrary.txt", testData);
        }
    }
}
