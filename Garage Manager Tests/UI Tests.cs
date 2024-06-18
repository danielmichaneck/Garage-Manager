using Garage_Manager;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager_Tests
{
    public class UI_Tests
    {
        IUI userInterface = new UI(outputMethod: Console.WriteLine,
                                   inputMethod: Console.ReadLine,
                                   clearMethod: Console.Clear);

        UI uiPrivate = new(outputMethod: Console.WriteLine,
                           inputMethod: Console.ReadLine,
                           clearMethod: Console.Clear);

        [Fact]
        public void Message_Struct_Test()
        {
            // Arrange
            string expected = "The file \"SavedList\" was successfully found in the .exe directory.";

            // Act
            string output = Message.ReadSuccess;


            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void UI_Print_Message_With_Specified_Action()
        {
            // Arrange
            Action<string> action = Console.WriteLine;
            StringWriter consoleOutput = new();
            string expected = Message.Start;
            Console.SetOut(consoleOutput);

            // Act
            userInterface.PrintMessage(Message.Start, a => action(Message.Start));

            // Assert
            Assert.Equal(expected, consoleOutput.ToString().Trim());
        }

        [Fact]
        public void UI_Print_Message_With_Member_Action()
        {
            // Arrange
            StringWriter consoleOutput = new();
            string expected = Message.Start;
            Console.SetOut(consoleOutput);

            // Act
            userInterface.PrintMessage(Message.Start);

            // Assert
            Assert.Equal(expected, consoleOutput.ToString().Trim());
        }

        [Fact]
        public void UI_GetInput_With_Specified_Action()
        {
            // Arrange
            string expected = Message.Start;
            string? input = "";
            Func<string?> function = new(() => Message.Start);

            // Act
            input = userInterface.GetInput(function);

            // Assert
            Assert.Equal(expected, input);
        }

        [Fact]
        public void UI_GetInput_With_Member_Action()
        {
            // Arrange
            string expected = Message.Start;
            string? input = "";
            Func<string?> function = new(() => Message.Start);
            userInterface = new UI(Console.WriteLine, function, Console.Clear);

            // Act

            input = userInterface.GetInput();

            // Assert
            Assert.Equal(expected, input);
        }

        [Fact]
        public void UI_GetValidInput_Success_Test()
        {
            // Arrange
            string expected = Message.Start;
            string? input = "";
            Func<string?> function = new(() => Message.Start);

            // Act
            input = userInterface.GetValidInput(function);

            // Assert
            Assert.Equal(expected, input);
        }

        [Fact]
        public void UI_GetValidInput_Fail_Test()
        {
            // Arrange
            string expected = Message.Start;
            Func<string?> function = new(() => " ");

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() => userInterface.GetValidInput(function));
        }

        [Fact]
        public void UI_CheckSameString_Success_Test()
        {
            // Arrange
            bool expected;
            string first = "This is a test.";
            string second = "ThiS IS a TeST.";

            // Act
            expected = uiPrivate.CheckIfSameString(first, second);

            // Assert
            Assert.True(expected);
        }

        [Fact]
        public void UI_CheckSameString_Fail_Test()
        {
            // Arrange
            bool expected;
            string first = "This is a test.";
            string second = "ThiS IS a fauLTY test!";

            // Act
            expected = uiPrivate.CheckIfSameString(first, second);

            // Assert
            Assert.False(expected);
        }

        [Fact]
        public void UI_GetValidBool_Success_Test()
        {
            // Arrange
            bool expected;
            Func<string?> function = new(() => "yEs");

            // Act
            expected = userInterface.GetValidBool(function);

            // Assert
            Assert.True(expected);
        }

        [Fact]
        public void UI_GetValidBool_Fail_Test()
        {
            // Arrange
            Func<string?> function = new(() => "yEsS");

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() => userInterface.GetValidInt(function));
        }

        [Fact]
        public void UI_GetValidInt_Success_Test()
        {
            // Arrange
            int expected;
            Func<string?> function = new(() => "1");

            // Act
            expected = userInterface.GetValidInt(function);

            // Assert
            Assert.True(expected != 0);
        }

        [Fact]
        public void UI_GetValidInt_Fail_Test()
        {
            // Arrange
            Func<string?> function = new(() => "A");

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() => userInterface.GetValidInt(function));
        }
    }
}
