using System;
using System.Collections.Generic;
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
                    CreateGarage();
                    break;
            }
        }

        private void CreateGarage()
        {
            _userInterface.PrintMessage(Message.CreateGarageSize);
            int size;
            while (!int.TryParse(_userInterface.GetValidInput(), out size));
            _userInterface.PrintMessage(Message.PopulateGarage);
            bool populate;
            while (!bool.TryParse(_userInterface.GetValidInput(), out populate));
            if (populate)
            {
                int numberOfVehicles = PopulateGarage(size);
                _userInterface.PrintMessage(Message.GarageCreated(size, numberOfVehicles) + Environment.NewLine);
            }
            else _userInterface.PrintMessage(Message.GarageCreated(size) + Environment.NewLine);
        }

        private int PopulateGarage(int size)
        {
            return 10;
        }
    }
}
