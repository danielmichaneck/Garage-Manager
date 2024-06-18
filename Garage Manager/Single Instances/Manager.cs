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
            _userInterface.PrintMessage(Message.Start);
            string input = _userInterface.GetValidInput();
            switch (input)
            {
                case "0":
                    _exit = true;
                    break;

                case "1":
                    ReadFromFile();
                    if (_fileReadSuccessfully)
                        AddContentsFromFile();
                    break;

                case "2":
                    _handler.CreateGarage(_userInterface.PrintMessage,
                                          _userInterface.GetValidInput,
                                          _userInterface.GetValidInt,
                                          _userInterface.GetValidBool,
                                          _userInterface.CheckIfSameString);
                    break;

                case "3":
                    _userInterface.PrintMessage(Environment.NewLine + _handler.ListAllVehiclesInAllGarages());
                    break;
            }
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
            IGarage<IVehicle> vehicles = new Garage<IVehicle>((_fileContents.Length / 2) - (garages.Count * 2));
            int garageIndex = -1;
            for (int i = 0; i < _fileContents.Length; i++)
            {

                if (_fileContents[i] == "New Garage")
                {
                    garageIndex++;
                }
                else if (_fileContents[i] == "New Vehicle")
                {
                    string[] vehicleInformation = _fileContents[i + 1].Split(',');
                    IVehicle vehicle = _handler.CreateVehicle(vehicles.ToArray(),
                                                              vehicleInformation[0],
                                                              vehicleInformation[1],
                                                              vehicleInformation[2],
                                                              _userInterface.CheckIfSameString)!;
                    garages.Get(garageIndex)!.Add(vehicle);
                    vehicles.Add(vehicle);
                }
            }
            // Adds the garages to the Handler's GarageList.
            for (int i = 0; i <  garages.Count; i++)
            {
                _handler.AddGarage(garages.Get(i)!);
            }
        }
    }
}
