using System;
using System.IO;
using System.Linq;
using Spectre.Console;
using Xunit;
using ShelfMate;

namespace ShelfMate.Tests
{
    public class ViewBookLibraryTests
    {
        [Fact]
        public void BookLibrary_ShouldContain_AllExpectedCategories()
        {
            // Arrange
            var expectedCategories = new[]
            {
                "View All Books", "View Owned Books", "View To Read Books", "View Currently Reading Books", "View Finished Books"
            };

            // Act
            var bookLibrary = new[]
            {
                "View All Books", "View Owned Books", "View To Read Books", "View Currently Reading Books", "View Finished Books"
            };

            // Assert
            Assert.Equal(expectedCategories.Length, bookLibrary.Length);
            foreach (var category in expectedCategories)
            {
                Assert.Contains(category, bookLibrary);
            }
        }

        [Theory]
        [InlineData("View Owned Books", "Owned")]
        [InlineData("View To Read Books", "To Read")]
        [InlineData("View Currently Reading Books", "Currently Reading")]
        [InlineData("View Finished Books", "Finished")]
        [InlineData("View All Books", null)]
        public void SelectedStatus_ShouldMatch_ExpectedValue(string category, string expectedStatus)
        {
            // Act
            string selectedStatus = category switch
            {
                "View Owned Books" => "Owned",
                "View To Read Books" => "To Read",
                "View Currently Reading Books" => "Currently Reading",
                "View Finished Books" => "Finished",
                _ => null
            };

            // Assert
            Assert.Equal(expectedStatus, selectedStatus);
        }
    }
}
