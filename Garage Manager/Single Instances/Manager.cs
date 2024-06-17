using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Garage Manager Tests")]

namespace Garage_Manager
{
    internal class Manager
    {
        private IUI _userInterface;
        private IHandler _handler;

        private bool exit = false;

        public Manager()
        {
            _userInterface = new UI(outputMethod: Console.WriteLine,
                                    inputMethod: Console.ReadLine);
            _handler = new Handler();
        }

        public void RunApplication()
        {
            do
            {
                MainMenu();

            } while (!exit);
        }

        private void MainMenu()
        {
            _userInterface.PrintMessage(Message.Start);
            string input = _userInterface.GetValidInput();
            switch (input)
            {
                case "0":
                    exit = true;
                    break;

                case "1":

                    break;

                case "2":
                    _handler.CreateGarage(_userInterface.PrintMessage,
                                          _userInterface.GetValidInput,
                                          _userInterface.GetValidInt,
                                          _userInterface.GetValidBool,
                                          _userInterface.CheckIfSameString);
                    break;

                case "3":
                    _userInterface.PrintMessage(_handler.ListAllVehiclesInAllGarages());
                    break;
            }
        }
    }
}
