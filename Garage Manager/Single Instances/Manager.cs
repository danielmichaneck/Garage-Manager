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
    /// <summary>
    /// Manager handles the basic functionality of the program
    /// including menus. It has a reference to a user interface
    /// for handling output and input as well as a reference
    /// to a handler for handling the garages and vehicles.
    /// </summary>
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

        // Lists the available functionality and accepts user input.
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
                    ListAllGarages();
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
                    ListSpecificVehiclesInSpecificGarage();
                    break;

                case "7":
                    CreateNewGarage();
                    break;

                case "8":
                    AccessGarage();
                    break;

                case "9":
                    FindVehicle();
                    break;

                default:
                    _userInterface.PrintMessage(Message.InputNotValid);
                    break;
            }
        }

        // Checks the number of garages and prints messages depending on
        // if there are none or only one.
        private int CheckNumberOfGarages()
        {
            _userInterface.ClearOutput();
            if (_handler.Count() == 0)
            {
                _userInterface.PrintMessage(Environment.NewLine + Message.ListNoGarages);
                return 0;
            }
            else if (_handler.Count() == 1)
            {
                _userInterface.PrintMessage(Environment.NewLine +
                                            Message.ListOnlyOneGarage +
                                            Environment.NewLine);
                return 1;
            }
            return _handler.Count();
        }

        // Accesses a specific garage with user input.
        private void AccessGarage()
        {
            int numberOfGarages = CheckNumberOfGarages();
            if (numberOfGarages < 1)
            {
                _userInterface.PrintMessage(Message.ListNoGarages);
                return;
            }
            IGarage<IVehicle>? garage;
            int index = 1;
            if (numberOfGarages == 1) garage = _handler.GetGarage(0)!;
            else
            {
                _userInterface.PrintMessage(Message.ListSpecificGarage(1, numberOfGarages));
                index = _userInterface.GetValidInt();
                garage = _handler.GetGarage(index - 1);
            }
            if (garage is null)
            {
                _userInterface.PrintMessage(Message.ListSpecificGarageDoesNotExist);
                return;
            }
            GarageMenu(index);
        }

        // Accesses a specific garage with user input.
        private void RemoveGarage()
        {
            int numberOfGarages = CheckNumberOfGarages();
            if (numberOfGarages < 1)
            {
                _userInterface.PrintMessage(Message.ListNoGarages);
                return;
            }
            IGarage<IVehicle>? garage;
            int index = 1;
            if (numberOfGarages == 1) garage = _handler.GetGarage(0)!;
            else
            {
                _userInterface.PrintMessage(Message.ListSpecificGarage(1, numberOfGarages));
                index = _userInterface.GetValidInt();
                garage = _handler.GetGarage(index - 1);
            }
            if (garage is null)
            {
                _userInterface.PrintMessage(Message.ListSpecificGarageDoesNotExist);
                return;
            }
            GarageMenu(index);
        }

        // Lists the available functionality for the garage
        // and accepts user input.
        private void GarageMenu(int index)
        {
            bool loop = true;
            do
            {
                _userInterface.PrintMessage("");
                _handler.ListGarage(index - 1, _userInterface.PrintMessage);
                _userInterface.PrintMessage(Message.GarageAccessed(index));
                _userInterface.PrintMessage("");
                int input = _userInterface.GetValidInt();
                _userInterface.ClearOutput();
                switch (input)
                {
                    case 0:
                        loop = false;
                        break;

                    case 1:
                        _userInterface.PrintMessage(Message.AccessingGarage(index));
                        _userInterface.PrintMessage(_handler.ListAllVehiclesInGarage(index - 1));
                        break;

                    case 2:
                        _userInterface.PrintMessage(Message.AccessingGarage(index));
                        CreateNewVehicle(index - 1);
                        break;

                    case 3:
                        _userInterface.PrintMessage(Message.AccessingGarage(index));
                        RemoveVehicle(index - 1);
                        break;
                }
            } while (loop);
        }

        // Finds a specific vehicle by its unique license number.
        private void FindVehicle()
        {
            if (CheckNumberOfGarages() > 0)
            {
                _userInterface.PrintMessage(Message.FindVehicle);
                string input = _userInterface.GetValidInput();
                IVehicle? vehicle = FindVehicleSearch(input, out int garageNumber);
                if (vehicle is not null)
                {
                    _userInterface.PrintMessage(Message.VehicleFound(garageNumber + 1) +
                                                Environment.NewLine +
                                                vehicle.GetVehicleInformation().ToString() +
                                                Environment.NewLine);
                }
                else _userInterface.PrintMessage(Message.VehicleNotFound);
            }
        }

        private IVehicle? FindVehicleSearch(string input, out int garageNumber)
        {
            garageNumber = 0;
            foreach(IGarage<IVehicle> garage in _handler.GetAllGarages())
            {
                foreach(IVehicle vehicle in garage)
                {
                    if (_userInterface.CheckIfSameString(input, vehicle.GetVehicleInformation().LicenseNumber))
                    {
                        return vehicle;
                    }
                }
                garageNumber++;
            }
            return null;
        }

        // Lists all garages and the frequency of vehicle types in them.
        private void ListAllGarages()
        {
            _handler.ListAllGarages(_userInterface.PrintMessage);
        }

        // Creates a new garage.
        private void CreateNewGarage()
        {
            _userInterface.ClearOutput();
            _handler.CreateGarage(_userInterface.PrintMessage,
                                  _userInterface.GetValidInput,
                                  _userInterface.GetValidInt,
                                  _userInterface.GetValidBool,
                                  _userInterface.CheckIfSameString);
        }

        // Creates a new vehicle and adds it to a garage.
        private void CreateNewVehicle(int? index = null)
        {
            _userInterface.ClearOutput();
            if (_handler.GetAllGarages().Count() < 1)
            {
                _userInterface.PrintMessage(Message.ListNoGarages);
            }
            else
            {
                if (_handler.GetAllGarages().Count() < 2)
                {
                    if (index != 0)
                    {
                        _userInterface.PrintMessage(Message.ListOnlyOneGarage);
                        index = 0;
                    }
                }
                else if (index is null)
                {
                    _userInterface.PrintMessage(Message.AddVehicleWhichGarage(1, _handler.GetAllGarages().Count()));
                    index = _userInterface.GetValidInt() - 1;
                }
                IVehicle vehicle = _handler.CreateVehicle(_userInterface.PrintMessage,
                                                          _userInterface.GetValidInput,
                                                          _userInterface.GetValidInt,
                                                          _userInterface.GetValidBool,
                                                          _userInterface.CheckIfSameString);
                _handler.AddVehicleToGarage(vehicle, (int)index, _userInterface.PrintMessage);
            }
        }

        // Removes a vehicle from a garage.
        private void RemoveVehicle(int index)
        {
            _userInterface.PrintMessage(Message.RemoveVehicle);
            string input = _userInterface.GetValidInput();
            if (_handler.RemoveVehicle(index, input, _userInterface.CheckIfSameString))
            {
                _userInterface.PrintMessage(Message.VehicleRemoved(input));
            }
            else _userInterface.PrintMessage(Message.VehicleRemoveFailed(input));
        }

        // Lists vehicle information for all vehicles
        // in all garages.
        private void ListAllVehiclesInAllGarages()
        {
            _userInterface.ClearOutput();
            _userInterface.PrintMessage(Environment.NewLine +
                                        _handler.ListAllVehiclesInAllGarages() +
                                        Environment.NewLine);
        }

        // Lists all vehicle information for all vehicles
        // in a specific garage.
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
            _userInterface.PrintMessage(Message.ListSpecificGarage(1, _handler.Count()) +
                                        Environment.NewLine);
            int input = _userInterface.GetValidInt();
            if (input < 1 || input > _handler.Count())
            {
                _userInterface.PrintMessage(Environment.NewLine + Message.ListSpecificGarageDoesNotExist);
                return;
            }
            _userInterface.PrintMessage($"Garage {input}" + Environment.NewLine +
                                        _handler.ListAllVehiclesInGarage(input - 1) +
                                        Environment.NewLine);
        }

        // Lists all vehicles in all garages meeting certain criteria
        // based on the vehicle's properties and defined by the user.
        private void ListSpecificVehiclesInAllGarages()
        {
            _userInterface.ClearOutput();
            // Gets all vehicles.
            List<IVehicle> vehicleList = new List<IVehicle>();
            if (_handler.GetAllGarages().Count() < 1)
            {
                _userInterface.PrintMessage(Message.ListNoGarages);
                return;
            }
            foreach (IGarage<IVehicle> garage in _handler.GetAllGarages())
            {
                foreach (IVehicle vehicle in garage)
                    vehicleList.Add(vehicle);
            }
            var vehicles = vehicleList.Select(v => v);
            ListSpecificVehiclesInGarage(vehicles);
        }

        // Lists all vehicles in a specific garage meeting certain criteria
        // based on the vehicle's properties and defined by the user.

        // ToDo: Shares a lot of copy-paste code with ListSpecificGarage(), fix?
        private void ListSpecificVehiclesInSpecificGarage()
        {
            // Sets index.
            int index = 1;
            _userInterface.ClearOutput();
            if (_handler.Count() < 1)
            {
                _userInterface.PrintMessage(Environment.NewLine + Message.ListNoGarages);
                return;
            }
            else if (_handler.Count() == 1)
            {
                _userInterface.PrintMessage(Environment.NewLine + Message.ListOnlyOneGarage);
                index = 1;
            }
            else
            {
                _userInterface.PrintMessage(Message.ListSpecificGarage(1, _handler.Count()) +
                                            Environment.NewLine);
                int input = _userInterface.GetValidInt();
                if (input < 1 || input > _handler.Count())
                {
                    _userInterface.PrintMessage(Environment.NewLine + Message.ListSpecificGarageDoesNotExist);
                    return;
                }
                else index = input;
            }
            // Gets all vehicles.
            List<IVehicle> vehicleList = new List<IVehicle>(_handler.GetGarage(index - 1)!);
            var vehicles = vehicleList.Select(v => v);
            ListSpecificVehiclesInGarage(vehicles);
        }

        // The main method for the selection process. It presents options for
        // how to select certain vehicles based on different properties and calls
        // the appropriate methods. It loops until the user is done with their
        // selections and then displays the vehicles matching the criteria.
        private void ListSpecificVehiclesInGarage(IEnumerable<IVehicle> vehicles)
        {
            // Loops through input and selection.
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
                        vehicles = SelectByInteger(vehicles, "length in meters");
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
                    // ToDo: Last-minute solution! Was trying to reduce unnecessary code...
                    if (property == "number of wheels")
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
                    else switch (input)
                    {
                        case 1:
                            return vehicles.Where(vehicle => vehicle.GetVehicleInformation().Size == input)
                                           .Select(v => v);

                        case 2:
                            return vehicles.Where(vehicle => vehicle.GetVehicleInformation().Size <= input)
                                           .Select(v => v);

                        case 3:
                            return vehicles.Where(vehicle => vehicle.GetVehicleInformation().Size >= input)
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

        // Reads from a file at the same location as the .exe-file.
        // The file must be named "SavedList.txt".
        // The file can contain a number of garages and their sizes
        // as well as the vehicles stored in those garages.
        // Only basic vehicle properties such as the license number
        // and color are currently supported.
        // An example of the syntax in the file is found at the bottom
        // of this file as a comment.
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
                else _userInterface.PrintMessage(Message.ReadNotAddingContents);
            }
            _userInterface.PrintMessage("");
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

        // The user chooses whether to add the contents of the file to
        // the memory. Currently there is no way to replace what is already
        // in memory or to write information to the file.
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

/*
New Garage creates a new garage and the following line is its size.
New Vehicle creates a new vehicle and the following line is its basic properties.
Vehicles are added to the previously created Garage.

The text below will generate 4 garages with 8 vehicles in them in total.

The second garage, with a size of 1, will be empty because the vehicle in it
has a vehicle type that is not recognized by the program. It will therefore not
be created and added to the garage.

New Garage
4
New Vehicle
Car,CAR 123,Blue
New Vehicle
Car,BIL 456,Red
New Garage
1
New Vehicle
Cat,Car 003,321
New Garage
2
New Vehicle
Airplane,AIR 117,White
New Vehicle
Car,CAR 789,white
New Garage
10
New Vehicle
Motorcycle,VROOM 1,blue
New Vehicle
Trike,WOW 124,red
New Vehicle
Boat,SEA 950,red
New Vehicle
Bus,BUS 001,white
*/