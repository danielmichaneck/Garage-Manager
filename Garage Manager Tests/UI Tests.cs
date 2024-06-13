﻿using Garage_Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager_Tests
{
    public class UI_Tests
    {
        IUI UserInterface = new UI(outputMethod: Console.WriteLine,
                                   inputMethod: Console.ReadLine);

        [Fact]
        public void Message_Struct_Test()
        {
            // Arrange
            string expected = "Please select an option from the menu." +
            "1. Read data from a file." +
            "2. Create a new garage." +
            "3. Exit the application.";

            // Act
            string output = Message.Start;


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
            UserInterface.PrintMessage(Message.Start, a => action(Message.Start));

            // Assert
            Assert.Equal(expected, consoleOutput.ToString().Trim());
        }

        [Fact]
        public void UI_Print_Message_With_Member_Action()
        {
            // Arrange
            Action<string> action = Console.WriteLine;
            StringWriter consoleOutput = new();
            string expected = Message.Start;
            Console.SetOut(consoleOutput);

            // Act
            UserInterface.PrintMessage(Message.Start, a => action(Message.Start));

            // Assert
            Assert.Equal(expected, consoleOutput.ToString().Trim());
        }
    }
}
