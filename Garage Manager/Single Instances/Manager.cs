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

        private bool _exit = false;

        private bool _fileReadSuccessfully;
        private string[] _fileContents;

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

            } while (!_exit);
        }

        private void MainMenu()
        {
            _userInterface.PrintMessage(Environment.NewLine + Message.Start + Environment.NewLine);
            string input = _userInterface.GetValidInput();
            Console.Clear();
            switch (input)
            {
                case "0":
                    _exit = true;
                    break;

                case "1":
                    ReadFromFileStart();
                    break;

                case "2":
                    CreateNewGarage();
                    break;

                case "3":
                    ListAllVehiclesInAllGarages();
                    break;

                case "4":
                    ListSpecificGarage();
                    break;
            }
        }

        private void CreateNewGarage()
        {
            _handler.CreateGarage(_userInterface.PrintMessage,
                                          _userInterface.GetValidInput,
                                          _userInterface.GetValidInt,
                                          _userInterface.GetValidBool,
                                          _userInterface.CheckIfSameString);
        }

        private void ListAllVehiclesInAllGarages()
        {
            _userInterface.PrintMessage(Environment.NewLine +
                                                _handler.ListAllVehiclesInAllGarages() +
                                                Environment.NewLine);
        }

        private void ListSpecificGarage()
        {
            if (_handler.Count() == 0)
            {
                _userInterface.PrintMessage(Environment.NewLine + Message.ListNoGarages);
                return;
            }
            else if (_handler.Count() == 1)
            {
                _userInterface.PrintMessage(Environment.NewLine + Message.ListOnlyOneGarage);
                _userInterface.PrintMessage(Environment.NewLine +
                                            _handler.ListAllVehiclesInGarage(0) +
                                            Environment.NewLine);
                return;
            }
            _userInterface.PrintMessage(Message.ListSpecificGarage(1, _handler.Count()));
            int input = _userInterface.GetValidInt();
            if (input < 0 || input >= _handler.Count())
            {
                _userInterface.PrintMessage(Environment.NewLine + Message.ListSpecificGarageDoesNotExist);
                return;
            }
            _userInterface.PrintMessage(Environment.NewLine +
                                        _handler.ListAllVehiclesInGarage(input - 1) +
                                        Environment.NewLine);
        }

        private void ReadFromFileStart()
        {
            ReadFromFile();
            if (_fileReadSuccessfully)
                AddContentsFromFile();
        }

        private bool ReadFromFile()
        {
            string directory = Directory.GetCurrentDirectory();
            string path = directory + @"\SavedList.txt";
            if (File.Exists(path))
            {
                _fileContents = File.ReadAllLines(path);
                if (_fileContents is not null && !String.IsNullOrWhiteSpace(_fileContents[0])) _fileReadSuccessfully = true;
                else _userInterface.PrintMessage(Message.ReadNull);
            }
            else _userInterface.PrintMessage(Message.ReadNotFound);
            if (_fileReadSuccessfully)
            {
                _userInterface.PrintMessage(Message.ReadSuccess);
                return true;
            }
            return false;
        }

        private void AddContentsFromFile()
        {
            // Setting up garages.
            GarageList<IGarage<IVehicle>> garages = new GarageList<IGarage<IVehicle>>();
            for (int i = 0; i < _fileContents.Length; i++)
            {
                if (_fileContents[i] == "New Garage")
                {
                    garages.Add(new Garage<IVehicle>(int.Parse(_fileContents[i + 1])));
                }
            }
            // Checking for vehicles and populating garages.
            IGarage<IVehicle> vehicles = new Garage<IVehicle>(_fileContents.Length / 2);
            int garageIndex = -1;
            for (int i = 0; i < _fileContents.Length; i++)
            {

                if (_fileContents[i] == "New Garage")
                {
                    garageIndex++;
                    i++;
                }
                else if (_fileContents[i] == "New Vehicle")
                {
                    string[] vehicleInformation = _fileContents[i + 1].Split(',');
                    IVehicle? vehicle = _handler.CreateVehicle(vehicles.ToArray(),
                                                               vehicleInformation[0],
                                                               vehicleInformation[1],
                                                               vehicleInformation[2],
                                                               _userInterface.CheckIfSameString)!;
                    if (vehicle is null) _userInterface.PrintMessage(Message.VehicleNull(i + 1));
                    else
                    {
                        garages.Get(garageIndex)!.Add(vehicle);
                        vehicles.Add(vehicle);
                    }
                    i++;
                }
            }
            // Adds the garages to the Handler's GarageList.
            for (int i = 0; i <  garages.Count; i++)
            {
                _handler.AddGarage(garages.Get(i)!);
            }
            _userInterface.PrintMessage(Message.FinishedAddingContent(garages.Count(), vehicles.Count()));
        }
    }
}
