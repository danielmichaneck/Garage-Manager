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
                    CreateGarage();
                    break;

                case "3":
                    _userInterface.PrintMessage(_handler.ListAllVehiclesInAllGarages());
                    break;
            }
        }

        private void CreateGarage()
        {
            _userInterface.PrintMessage(Message.CreateGarageSize);
            int size = _userInterface.GetValidInt();
            _userInterface.PrintMessage(Message.PopulateGarage);
            bool populate = _userInterface.GetValidBool();
            if (populate)
            {
                int numberOfVehicles = PopulateGarage(size);
                _userInterface.PrintMessage(Message.GarageCreated(size, numberOfVehicles) + Environment.NewLine);
            }
            else _userInterface.PrintMessage(Message.GarageCreated(size) + Environment.NewLine);
        }

        private int PopulateGarage(int size)
        {
            _userInterface.PrintMessage(Message.NumberOfVehicles);
            int numberOfVehicles = 0;
            do
            {
                numberOfVehicles = _userInterface.GetValidInt();
            } while (numberOfVehicles > size);
            IVehicle[] newVehicles = new IVehicle[numberOfVehicles];
            for (int i = 0; i < numberOfVehicles; i++)
            {
                _userInterface.PrintMessage(Message.PopulateVehicleType);
                bool loop = true;
                int repeats = 0;
                VehicleType[] vehicleTypes = [VehicleType.Car];
                VehicleType vehicleType = VehicleType.Car;
                string input;
                do
                {
                    input = _userInterface.GetValidInput();
                    for (int p = 0; p < vehicleTypes.Length; p++)
                    {
                        if (_userInterface.CheckIfSameString(input, vehicleTypes[p].ToString()))
                        {
                            vehicleType = vehicleTypes[p];
                            loop = false;
                            break;
                        }
                    }
                    repeats++;
                    if (repeats > 100) throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
                    if (loop) _userInterface.PrintMessage(Message.InputNotValid);
                } while (loop);
                //string licenseNumber = _userInterface.GetValidInput();
                //string color = _userInterface.GetValidInput();
                //Color newColor = Color.FromName(color);
                newVehicles[i] = new Car("licenseNumber", Color.Blue);
            }
            _handler.CreateNewGarage(size, newVehicles);
            return numberOfVehicles;
        }
    }
}
