using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
                    WriteToFile();
                    break;

                case "10":
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

                case "6":
                    CreateNewVehicle();
                    break;

                case "7":
                    ListAllGarages();
                    break;
            }
        }

        private void ListAllGarages()
        {
            _handler.ListAllGarages(_userInterface.PrintMessage);
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

        private void CreateNewVehicle()
        {
            _userInterface.ClearOutput();
            if (_handler.GetAllGarages().Count() < 1)
            {
                _userInterface.PrintMessage(Message.ListNoGarages);
            }
            else
            {
                int index;
                if (_handler.GetAllGarages().Count() < 2)
                {
                    _userInterface.PrintMessage(Message.ListOnlyOneGarage);
                    index = 0;
                }
                else
                {
                    _userInterface.PrintMessage(Message.AddVehicleWhichGarage(1, _handler.GetAllGarages().Count()));
                    index = _userInterface.GetValidInt() - 1;
                }
                IVehicle vehicle = _handler.CreateVehicle(_userInterface.PrintMessage,
                                                          _userInterface.GetValidInput,
                                                          _userInterface.GetValidInt,
                                                          _userInterface.GetValidBool,
                                                          _userInterface.CheckIfSameString);
                _handler.AddVehicleToGarage(vehicle, index, _userInterface.PrintMessage);
            }
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
            // Gets all vehicles.
            List<IVehicle> vehicleList = new List<IVehicle>();
            foreach (IGarage<IVehicle> garage in _handler.GetAllGarages())
            {
                foreach (IVehicle vehicle in garage)
                    vehicleList.Add(vehicle);
            }
            var vehicles = vehicleList.Select(v => v);
            // Gets index.
            bool loop = true;
            do
            {
                _userInterface.PrintMessage(Message.SpecificVehicleByProperty());
                string input = _userInterface.GetValidInput();
                switch (input)
                {
                    case "0":
                        loop = false;
                        break;

                    case "1":
                        vehicles = SelectByVehicleType(vehicles);
                        break;

                    case "2":
                        vehicles = SelectByColor(vehicles);
                        break;

                    case "3":
                        vehicles = SelectByInteger(vehicles, "size as number of parking spots");
                        break;

                    case "4":
                        vehicles = SelectByInteger(vehicles, "number of wheels");
                        break;

                    case "5":
                        vehicles = SelectByFuelType(vehicles);
                        break;
                }
            } while (loop);
            // Prints output
            _userInterface.ClearOutput();
            foreach (IVehicle vehicle in vehicles)
            {
                _userInterface.PrintMessage(vehicle.GetVehicleInformation().ToString() + Environment.NewLine);
            }
        }

        private IEnumerable<IVehicle> SelectByVehicleType(IEnumerable<IVehicle> vehicles)
        {
            int repeats = 0;
            do
            {
                _userInterface.PrintMessage(Message.SpecificVehicleEnterProperty("vehicle type"));
                int i = 1;
                foreach (VehicleType vehicleType in IVehicle.VehicleTypes)
                {
                    _userInterface.PrintMessage($"{i}. {IVehicle.VehicleTypes[i - 1]}." + Environment.NewLine);
                    i++;
                }
                _userInterface.PrintMessage(Environment.NewLine);
                int input = _userInterface.GetValidInt();
                if (input > 0 && input <= IVehicle.VehicleTypes.Count())
                    return vehicles.Where(vehicle => vehicle.GetVehicleInformation().Vehicletype == IVehicle.VehicleTypes[input - 1])
                                   .Select(v => v);
                else if (input == 0) return vehicles;
                repeats++;
            } while (repeats < 100);
            throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
        }

        private IEnumerable<IVehicle> SelectByColor(IEnumerable<IVehicle> vehicles)
        {
            int repeats = 0;
            Color color;
            do
            {
                _userInterface.PrintMessage(Message.SpecificVehicleEnterProperty("color"));
                _userInterface.PrintMessage(Environment.NewLine);
                string input = _userInterface.GetValidInput();
                color = Color.FromName(input);
                if (color.IsKnownColor)
                {
                    return vehicles.Where(vehicle => vehicle.GetVehicleInformation().Color == color)
                                   .Select(v => v);
                }
                else if (input == "0") return vehicles;
                repeats++;
            } while (repeats < 100);
            throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
        }

        private IEnumerable<IVehicle> SelectByInteger(IEnumerable<IVehicle> vehicles, string property)
        {
            int repeats = 0;
            do
            {
                _userInterface.PrintMessage(Message.SpecificVehicleEnterProperty(property));
                _userInterface.PrintMessage(Environment.NewLine);
                int input = _userInterface.GetValidInt();
                if (input > -1)
                {
                    _userInterface.PrintMessage(Message.SpecificVehicleEqualUpperOrLower(property));
                    _userInterface.PrintMessage(Environment.NewLine);
                    input = _userInterface.GetValidInt();
                    switch (input)
                    {
                        case 1:
                            return vehicles.Where(vehicle => vehicle.GetVehicleInformation().NumberOfWheels == input)
                                           .Select(v => v);

                        case 2:
                            return vehicles.Where(vehicle => vehicle.GetVehicleInformation().NumberOfWheels <= input)
                                           .Select(v => v);

                        case 3:
                            return vehicles.Where(vehicle => vehicle.GetVehicleInformation().NumberOfWheels >= input)
                                           .Select(v => v);
                    }
                    _userInterface.PrintMessage(Message.InputNotValid);
                }
                else if (input == 0) return vehicles;
                repeats++;
            } while (repeats < 100);
            throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
        }

        private IEnumerable<IVehicle> SelectByFuelType(IEnumerable<IVehicle> vehicles)
        {
            int repeats = 0;
            do
            {
                _userInterface.PrintMessage(Message.SpecificVehicleEnterProperty("fuel type"));
                int i = 1;
                foreach (FuelType fuelType in IVehicle.FuelTypes)
                {
                    _userInterface.PrintMessage($"{i}. {IVehicle.FuelTypes[i - 1]}." + Environment.NewLine);
                    i++;
                }
                _userInterface.PrintMessage(Environment.NewLine);
                int input = _userInterface.GetValidInt();
                if (input > 0 && input <= IVehicle.FuelTypes.Count())
                    return vehicles.Where(vehicle => vehicle.GetVehicleInformation().FuelType == IVehicle.FuelTypes[input - 1])
                                   .Select(v => v);
                else if (input == 0) return vehicles;
                repeats++;
            } while (repeats < 100);
            throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
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

        private bool WriteToFile()
        {
            string directory = Directory.GetCurrentDirectory();
            string path = directory + @"\SavedList.txt";
            if (File.Exists(path))
            {
                //_fileContents = File.ReadAllLines(path);
                //if (_fileContents is not null && !String.IsNullOrWhiteSpace(_fileContents[0])) _fileReadSuccessfully = true;
                //else _userInterface.PrintMessage(Message.ReadNull);
                FileStream fs = File.OpenWrite(path);
                File.WriteAllText(path, "Hej");
                fs.Close();
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
