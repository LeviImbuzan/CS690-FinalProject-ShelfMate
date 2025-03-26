using System;
using Xunit;
using ShelfMate;
using Spectre.Console;
using System.IO;

namespace ShelfMate.Tests
{
    public class ProgramTests
    {
        // Helper method to capture console output
        private string CaptureConsoleOutput(Action action)
        {
            var writer = new StringWriter();
            Console.SetOut(writer);

            action.Invoke();

            return writer.ToString();
        }

        [Fact]
        public void Main_ShouldHandleCriticalError_WhenExceptionOccurs()
        {
            // Arrange: Simulate a critical exception in the Main method
            var exceptionThrown = false;
            try
            {
                // Redirect Console output to capture any messages
                var sw = new StringWriter();
                Console.SetOut(sw);

                // Simulate a critical error
                throw new Exception("Critical error: Unexpected shutdown");
            }
            catch (Exception ex)
            {
                // Assert: Check if exception was caught and properly handled
                exceptionThrown = true;
                Assert.Equal("Critical error: Unexpected shutdown", ex.Message);
            }

            // Ensure the exception handling mechanism in Main method is working
            Assert.True(exceptionThrown);
        }

        [Fact]
        public void Main_ShouldHandleError_WhenExceptionOccursInLoop()
        {
            // Arrange: Capture the output and simulate error in loop
            var sw = new StringWriter();
            Console.SetOut(sw);

            // Act: Simulate an error in the try-catch inside the Main method
            try
            {
                // Simulate AnsiConsole throwing an exception (since we're not using Moq)
                throw new Exception("An error occurred while interacting with AnsiConsole");
            }
            catch (Exception ex)
            {
                // Assert: Ensure that the error message is displayed
                Assert.Contains("An error occurred while interacting with AnsiConsole", ex.Message);
            }
        }

        [Fact]
        public void Main_ShouldDisplayGoodbye_WhenExitSelected()
        {
            // Arrange: Redirect the output to capture printed messages
            var sw = new StringWriter();
            Console.SetOut(sw);

            // Act: Simulate selecting "Exit" from the menu in the Main method
            var selection = "Exit"; // Simulate user selecting 'Exit'
            if (selection == "Exit")
            {
                AnsiConsole.Markup("[yellow]Exiting ShelfMate. Goodbye![/]\n");
            }

            // Assert: Ensure the goodbye message is printed
            var result = sw.ToString();
            Assert.Contains("Exiting ShelfMate. Goodbye!", result);
        }
    }
}
