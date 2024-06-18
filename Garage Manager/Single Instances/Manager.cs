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
                                    inputMethod: Console.ReadLine,
                                    clearMethod: Console.Clear);
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
            //ToDo: Remove other clear outputs? Figure out why it does not work sometimes?
            _userInterface.ClearOutput();
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

                case "5":
                    ListSpecificVehiclesInAllGarages();
                    break;
            }
        }

        private void CreateNewGarage()
        {
            _userInterface.ClearOutput();
            _handler.CreateGarage(_userInterface.PrintMessage,
                                  _userInterface.GetValidInput,
                                  _userInterface.GetValidInt,
                                  _userInterface.GetValidBool,
                                  _userInterface.CheckIfSameString);
        }

        private void ListAllVehiclesInAllGarages()
        {
            _userInterface.ClearOutput();
            _userInterface.PrintMessage(Environment.NewLine +
                                                _handler.ListAllVehiclesInAllGarages() +
                                                Environment.NewLine);
        }

        private void ListSpecificGarage()
        {
            _userInterface.ClearOutput();
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
            if (input < 1 || input > _handler.Count())
            {
                _userInterface.PrintMessage(Environment.NewLine + Message.ListSpecificGarageDoesNotExist);
                return;
            }
            _userInterface.PrintMessage(Environment.NewLine +
                                        _handler.ListAllVehiclesInGarage(input - 1) +
                                        Environment.NewLine);
        }

        private void ListSpecificVehiclesInAllGarages()
        {
            _userInterface.ClearOutput();
            // Gets all vehicleList.
            List<IVehicle> vehicleList = new List<IVehicle>();
            foreach (IGarage<IVehicle> garage in _handler.GetAllGarages())
            {
                foreach (IVehicle vehicle in garage)
                    vehicleList.Add(vehicle);
            }
            var vehicles = vehicleList.Select(v => v);
            // Gets input.
            bool answerAsBool;
            string answerAsString;
            _userInterface.PrintMessage(Message.SpecificVehicleByProperty("Vehicle Type"));
            answerAsBool = _userInterface.GetValidBool();
            if (answerAsBool)
            {
                bool finished = false;
                do
                {
                    _userInterface.PrintMessage(Message.SpecificVehicleEnterProperty("Vehicle Type"));
                    answerAsString = _userInterface.GetValidInput();
                    VehicleType? vehicleType = IVehicle.GetVehicleType(answerAsString, _userInterface.CheckIfSameString);
                    vehicles = vehicles.Where(vehicle => vehicle.GetVehicleInformation()._vehicletype == vehicleType)
                                       .Select(v => v);
                    _userInterface.PrintMessage(Message.SpecificVehicleAnotherProperty("Vehicle Type"));
                    finished = !_userInterface.GetValidBool();
                } while (!finished);
            }
            _userInterface.PrintMessage(Message.SpecificVehicleByProperty("Color"));
            answerAsBool = _userInterface.GetValidBool();
            if (answerAsBool)
            {
                bool finished = false;
                do
                {
                    _userInterface.PrintMessage(Message.SpecificVehicleEnterProperty("Color"));
                    answerAsString = _userInterface.GetValidInput();
                    //ToDo: Reuse code from Handler class?
                    Color color = Color.FromName(answerAsString);
                    vehicles = vehicles.Where(vehicle => vehicle.GetVehicleInformation()._color == color)
                                       .Select(v => v);
                    _userInterface.PrintMessage(Message.SpecificVehicleAnotherProperty("Color"));
                    finished = !_userInterface.GetValidBool();
                } while (!finished);
            }


            _userInterface.ClearOutput();
            foreach(IVehicle vehicle in vehicles)
            {
                _userInterface.PrintMessage(vehicle.GetVehicleInformation().ToString() + Environment.NewLine);
            }









            /*
            StringBuilder result = new();
            GarageList<IGarage<IVehicle>> garages = _handler.GetAllGarages();
            foreach (var garage in garages)
            {
                var vehicleList = garage.Select(v => v);
                foreach(IVehicle vehicle in vehicleList)
                {
                    result.Append(vehicle.GetVehicleInformation().ToString());
                }
            }*/
        }

        private void ReadFromFileStart()
        {
            _userInterface.ClearOutput();
            ReadFromFile();
            if (_fileReadSuccessfully)
            {
                _userInterface.PrintMessage(Message.ReadAddContents);
                bool input = _userInterface.GetValidBool();
                if (input)
                {
                    _userInterface.PrintMessage(Message.ReadAddingContents);
                    AddContentsFromFile();
                }
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
            // Checking for vehicleList and populating garages.
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
                    if (vehicle is null) _userInterface.PrintMessage(Message.ReadVehicleNull(i + 1));
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
            _userInterface.PrintMessage(Message.ReadFinishedAddingContent(garages.Count(), vehicles.Count()));
        }
    }
}
