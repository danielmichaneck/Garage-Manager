using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Handler Tests")]

namespace Garage_Manager
{
    /// <summary>
    /// A class that handles Garage instances and instancing of vehicles
    /// to put in those garages.
    /// </summary>
    internal class Handler : IHandler
    {
        private GarageList<IGarage<IVehicle>> _garages;

        public Handler()
        {
            _garages = new();
        }

        public void AddGarage(IGarage<IVehicle> garage)
        {
            _garages.Add(garage);
        }

        public IGarage<IVehicle>? GetGarage(int index)
        {
            if (_garages.Count > index && _garages.Get(index) != default)
            {
                return _garages.Get(index);
            }
            return default;
        }

        public GarageList<IGarage<IVehicle>> GetAllGarages()
        {
            return _garages;
        }

        public int Count()
        {
            return _garages.Count;
        }

        // Creates a garage with a specific size and a number of vehicles in it.
        public IGarage<IVehicle> CreateNewGarage(int size, IVehicle[] cars)
        {
            IGarage<IVehicle> newGarage = new Garage<IVehicle>(size);
            for (int i = 0; i < cars.Length; i++)
            {
                newGarage.Add(cars[i]);
            }
            AddGarage(newGarage);
            return newGarage;
        }

        // Creates an empty garage with a specific size.
        public IGarage<IVehicle> CreateNewGarage(int size)
        {
            var newGarage = CreateNewGarage(size, []);
            return newGarage;
        }

        // Lists the index of all garages and how many vehicles of each present
        // vehicle type is in that garage.
        public void ListAllGarages(Action<string> outputAction)
        {
            int index = 1;
            StringBuilder stringBuilder = new();
            int numberOfVehicleType;
            foreach(IGarage<IVehicle> garage in _garages)
            {
                outputAction.Invoke($"Garage {index}");
                foreach(VehicleType vehicleType in IVehicle.VehicleTypes)
                {
                    numberOfVehicleType = 0;
                    foreach(IVehicle vehicle in garage)
                    {
                        if (vehicle.GetVehicleInformation().Vehicletype == vehicleType)
                            numberOfVehicleType++;
                    }
                    if (numberOfVehicleType != 0)
                    {
                        if (stringBuilder.Length > 0) stringBuilder.Append(", ");
                        stringBuilder.Append($"{vehicleType}s: {numberOfVehicleType}");
                    }
                }
                index++;
                if (stringBuilder.Length < 1)
                    outputAction.Invoke(Message.ListEmptyGarage + Environment.NewLine);
                else outputAction.Invoke(stringBuilder.ToString() + Environment.NewLine);
                stringBuilder.Clear();
            }
        }

        // Lists the index of a specific garage and how many vehicles of each present
        // vehicle type is in that garage.

        // ToDo: Copy-paste from ListAllGarages, re-use code!
        public void ListGarage(int index, Action<string> outputAction)
        {
            StringBuilder stringBuilder = new();
            int numberOfVehicleType;
            if (_garages.Get(index) is null)
            {
                outputAction.Invoke(Message.ListSpecificGarageDoesNotExist);
            }
            outputAction.Invoke($"Garage {index + 1}");
            foreach (VehicleType vehicleType in IVehicle.VehicleTypes)
            {
                numberOfVehicleType = 0;
                foreach (IVehicle vehicle in _garages.Get(index)!)
                {
                    if (vehicle.GetVehicleInformation().Vehicletype == vehicleType)
                        numberOfVehicleType++;
                }
                if (numberOfVehicleType != 0)
                {
                    if (stringBuilder.Length > 0) stringBuilder.Append(", ");
                    stringBuilder.Append($"{vehicleType}s: {numberOfVehicleType}");
                }
            }
            if (stringBuilder.Length < 1)
                outputAction.Invoke(Message.ListEmptyGarage + Environment.NewLine);
            else outputAction.Invoke(stringBuilder.ToString() + Environment.NewLine);
        }

        // Returns the vehicle information of all vehicles in a specific garage as a string.
        private string ListVehicleInformation(IGarage<IVehicle> garage)
        {
            StringBuilder listStringBuilder = new();
            foreach (IVehicle vehicle in garage)
            {
                listStringBuilder.Append(vehicle.GetVehicleInformation().ToString() + Environment.NewLine
                                                                                    + Environment.NewLine);
            }
            return listStringBuilder.ToString().Trim();
        }

        // Returns the vehicle information of all vehicles in a specific garage as a string.
        public string ListAllVehiclesInGarage(int index)
        {
            IGarage<IVehicle>? garage = _garages.Get(index);
            if (garage is null) return Message.ListSpecificGarageDoesNotExist;
            else if (garage.Count() < 1) return Message.ListEmptyGarage;
            return ListVehicleInformation(garage);
        }

        // Returns the vehicle information of all vehicles in all garages as a string.
        public string ListAllVehiclesInAllGarages()
        {
            StringBuilder listStringBuilder = new("");
            int i = 0;
            foreach (IGarage<IVehicle> garage in _garages)
            {
                listStringBuilder.Append(Environment.NewLine + Environment.NewLine + $"Garage {++i}" + Environment.NewLine);
                if (garage.Count() < 1) listStringBuilder.Append(Message.ListEmptyGarage + Environment.NewLine);
                listStringBuilder.Append(ListVehicleInformation(garage).Trim() + Environment.NewLine);
            }
            return listStringBuilder.ToString().Trim();
        }

        // Creates an instance of a vehicle from strings.
        public IVehicle? CreateVehicle(IVehicle[] currentVehicles,
                                       string vehicleTypeAsString,
                                       string licenseNumber,
                                       string colorAsString,
                                       Func<string, string, bool> compareStringsFunc)
        {
            // Sets vehicle type.
            VehicleType? vehicleType = IVehicle.GetVehicleType(vehicleTypeAsString, compareStringsFunc);
            if (vehicleType is null) return null;
            // Checks that the license number is unique.
            if (!CheckLicenseNumber(currentVehicles, licenseNumber, compareStringsFunc)) return null;
            // Sets color.
            Color? color = GetColor(colorAsString);
            if (color is null) return null;
            // Returns instance.
            return InstantiateVehicle((VehicleType)vehicleType, licenseNumber, (Color)color);
        }

        // Creates an instance of a vehicle with user input.
        public IVehicle CreateVehicle(Action<string> outputAction,
                                      Func<string> inputFunc,
                                      Func<int> inputFuncInt,
                                      Func<bool> inputFuncBool,
                                      Func<string, string, bool> compareStringsFunc)
        {
            return GetVehicleWithProperties([],
                                            outputAction,
                                            inputFunc,
                                            inputFuncInt,
                                            inputFuncBool,
                                            compareStringsFunc);
        }

        // Creates a new garage with a set size and the option to populate it.
        public void CreateGarage(Action<string> outputAction,
                                  Func<string> inputFunc,
                                  Func<int> inputFuncInt,
                                  Func<bool> inputFuncBool,
                                  Func<string, string, bool> compareStringsFunc)
        {
            // Gets input to set garage size.
            outputAction.Invoke(Message.CreateGarageSize);
            int size = inputFuncInt.Invoke();
            // Gets input to see whether populate the garage or not.
            outputAction.Invoke(Message.PopulateGarage);
            bool populate = inputFuncBool.Invoke();
            if (populate)
            {
                // Populates and creates a garage.
                int numberOfVehicles = PopulateGarage(size, outputAction, inputFunc, inputFuncInt, inputFuncBool, compareStringsFunc);
                outputAction.Invoke(Message.GarageCreated(size, numberOfVehicles) + Environment.NewLine);
            }
            // Creates garage without populating it.
            else
            {
                CreateNewGarage(size);
                outputAction.Invoke(Message.GarageCreated(size) + Environment.NewLine);
            }
        }

        // Populates the garage with vehicles.
        private int PopulateGarage(int size, Action<string> outputAction,
                                    Func<string> inputFunc,
                                    Func<int> inputFuncInt,
                                    Func<bool> inputFuncBool,
                                    Func<string, string, bool> compareStringsFunc)
        {
            
            int numberOfVehicles = GetNumberOfVehicles(size, outputAction, inputFuncInt);
            IVehicle[] newVehicles = new IVehicle[numberOfVehicles];
            for (int i = 0; i < numberOfVehicles; i++)
            {
                newVehicles[i] = GetVehicleWithProperties(newVehicles,
                                                          outputAction,
                                                          inputFunc,
                                                          inputFuncInt,
                                                          inputFuncBool,
                                                          compareStringsFunc);
            }
            CreateNewGarage(size, newVehicles);
            return numberOfVehicles;
        }

        // Lets the user choose whether to specify only the basic properties
        // or all properties of the vehicle.
        private IVehicle GetVehicleWithProperties(IVehicle[] vehicles,
                                                  Action<string> outputAction,
                                                  Func<string> inputFunc,
                                                  Func<int> inputFuncInt,
                                                  Func<bool> inputFuncBool,
                                                  Func<string, string, bool> compareStringsFunc)
        {
            outputAction(Message.AllPropertiesOfVehicles);
            bool answer;
            answer = inputFuncBool.Invoke();
            if (answer)
            {
                return CreateVehicleWithAllProperties(vehicles,
                                                      outputAction,
                                                      inputFunc,
                                                      inputFuncInt,
                                                      inputFuncBool,
                                                      compareStringsFunc);
            }
            else
            {
                return CreateVehicle(vehicles,
                                     outputAction,
                                     inputFunc,
                                     compareStringsFunc);
            }
        }

        // Lets the user set how many vehicles they want to populate the garage with.
        private int GetNumberOfVehicles(int size,
                                        Action<string> outputAction,
                                        Func<int> inputFuncInt)
        {
            int numberOfVehicles;
            int repeats = 0;
            do
            {
                outputAction.Invoke(Message.NumberOfVehicles);
                numberOfVehicles = inputFuncInt.Invoke();
                if (numberOfVehicles > size)
                    outputAction(Message.NumberOfVehiclesTooMany);
                repeats++;
                if (repeats >= 100) throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
            } while (numberOfVehicles > size);
            return numberOfVehicles;
        }

        // Accesses the additional properties which are stored as strings
        // in the vehicle's VehicleInformation struct. It splits the string
        // into the name of the property and the value itself. It then checks
        // whether each value can be interpreted as an int, a bool, or a string
        // and then accepts input to overwrite the default value and set new
        // properties.
        private string[] GetAdditionalProperties(IVehicle vehicle,
                                                 Action<string> outputAction,
                                                 Func<string> inputFunc,
                                                 Func<int> inputFuncInt,
                                                 Func<bool> inputFuncBool)
        {
            VehicleInformation vehicleInformation = vehicle.GetVehicleInformation();
            string[] additionalProperties = vehicleInformation.GetAdditionalProperties() ?? Array.Empty<string>();
            if (additionalProperties.Length > 0)
            {
                for (int i = 0; i < additionalProperties.Length; i++)
                {
                    var propertyString = additionalProperties[i].Split(": ");
                    string property = propertyString[0];
                    string newValue;
                    if (int.TryParse(propertyString[1], out _))
                    {
                        outputAction.Invoke(Message.SetPropertyOfVehicle(property, "an integer"));
                        int integer = inputFuncInt.Invoke();
                        newValue = integer.ToString();
                    }
                    else if (bool.TryParse(propertyString[1], out _))
                    {
                        outputAction.Invoke(Message.SetPropertyOfVehicle(property, "true or false"));
                        bool boolean = inputFuncBool.Invoke();
                        newValue = boolean.ToString();
                    }
                    else
                    {
                        outputAction.Invoke(Message.SetPropertyOfVehicle(property, "a string of letters"));
                        newValue = inputFunc.Invoke();
                    }
                    additionalProperties[i] = property + ": " + newValue;
                }
            }
            return additionalProperties;
        }

        // Creates a vehicle using all of the standard vehicle properties
        // as well as any additional properties unique to that derived class.

        // ToDo: Copy-paste from the basic one!
        private IVehicle CreateVehicleWithAllProperties(IVehicle[] currentVehicles,
                                                        Action<string> outputAction,
                                                        Func<string> inputFunc,
                                                        Func<int> inputFuncInt,
                                                        Func<bool> inputFuncBool,
                                                        Func<string, string, bool> compareStringsFunc)
        {
            // Set the type of the vehicle.
            VehicleType vehicleType = GetVehicleType(outputAction,
                                                     inputFunc,
                                                     compareStringsFunc);
            // Set the license number of the vehicle.
            string licenseNumber = GetLicenseNumber(currentVehicles,
                                                    outputAction,
                                                    inputFunc,
                                                    compareStringsFunc);
            // Set the color of the vehicle.
            Color color = GetColor(outputAction,
                                   inputFunc,
                                   compareStringsFunc);
            // Set the optional values.
            var values = GetRestOfProperties(outputAction, inputFuncInt);
            FuelType fuelType = GetFuelType(outputAction, inputFuncInt);
            // Instantiate the vehicle.
            IVehicle vehicle = InstantiateVehicle(vehicleType,
                                      licenseNumber,
                                      color,
                                      values[0],
                                      values[1],
                                      values[2],
                                      fuelType);
            // Set additional properties if the vehicle has any.
            var additionalProperties = GetAdditionalProperties(vehicle,
                                                               outputAction,
                                                               inputFunc,
                                                               inputFuncInt,
                                                               inputFuncBool);

            vehicle.GetVehicleInformation().SetAdditionalProperties(additionalProperties);
            // Return the instanced vehicle.
            return vehicle;
        }

        /// <summary>
        /// Creates a new vehicle using user input.
        /// </summary>
        /// <param name="currentVehicles"></param>
        /// <param name="outputAction"></param>
        /// <param name="inputFunc"></param>
        /// <param name="compareStringsFunc"></param>
        /// <returns></returns>
        private IVehicle CreateVehicle(IVehicle[] currentVehicles,
                                       Action<string> outputAction,
                                       Func<string> inputFunc,
                                       Func<string, string, bool> compareStringsFunc)
        {
            VehicleType vehicleType = GetVehicleType(outputAction,
                                                     inputFunc,
                                                     compareStringsFunc);

            string licenseNumber = GetLicenseNumber(currentVehicles,
                                                    outputAction,
                                                    inputFunc,
                                                    compareStringsFunc);

            Color color = GetColor(outputAction,
                                   inputFunc,
                                   compareStringsFunc);

            return InstantiateVehicle(vehicleType, licenseNumber, color);
        }

        // Gets vehicle type from input.
        private VehicleType GetVehicleType(Action<string> outputAction,
                                            Func<string> inputFunc,
                                            Func<string, string, bool> compareStringsFunc)
        {
            string input;
            int repeats = 0;
            do
            {
                outputAction.Invoke(Message.InputVehicleType);
                input = inputFunc.Invoke();
                VehicleType? vehicleType = IVehicle.GetVehicleType(input, compareStringsFunc);
                if (vehicleType is not null) return (VehicleType)vehicleType;
                outputAction.Invoke(Message.InputVehicleTypeNotFound(input));
                repeats++;
            } while (repeats < 100);
            throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
        }

        // Gets a license number from input.
        private string GetLicenseNumber(IVehicle[] currentVehicles,
                                         Action<string> outputAction,
                                         Func<string> inputFunc,
                                         Func<string, string, bool> compareStringsFunc)
        {
            string licenseNumber;
            bool unique;
            int repeats = 0;
            do
            {
                outputAction.Invoke(Message.InputVehicleLicenseNumber);
                unique = true;
                licenseNumber = inputFunc.Invoke();
                unique = CheckLicenseNumber(currentVehicles, licenseNumber, compareStringsFunc);
                if (!unique) outputAction.Invoke(Message.InputVehicleLicenseNumberNotUnique);
                repeats++;
            } while (!unique && repeats < 100);
            if (repeats >= 100) throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
            return licenseNumber;
        }

        // Gets color from a string.
        private Color? GetColor(string colorAsString)
        {
            Color color = Color.FromName(colorAsString);
            if (!color.IsKnownColor) return null;
            return color;
        }

        // Gets color from input.
        private Color GetColor(Action<string> outputAction,
                                Func<string> inputFunc,
                                Func<string, string, bool> compareStringsFunc)
        {
            Color? color;
            int repeats = 0;
            do
            {
                outputAction.Invoke(Message.InputVehicleColor);
                color = GetColor(inputFunc.Invoke());
                if (color is null) outputAction.Invoke(Message.InputVehicleColorNotRecognized);
                repeats++;
                if (repeats >= 100) throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
            } while (color is null);
            return (Color)color!;
        }

        // Checks that a license number is unique using both the values stored
        // in all the vehicles in all the garages and the values in the
        // vehicles currently being instanced.
        private bool CheckLicenseNumber(IVehicle[] currentVehicles,
                                        string licenseNumber,
                                        Func<string, string, bool> compareStringsFunc)
        {
            foreach (Garage<IVehicle> garage in _garages)
            {
                foreach (IVehicle vehicle in garage)
                {
                    if (compareStringsFunc.Invoke(licenseNumber, vehicle.GetVehicleInformation().LicenseNumber))
                    {
                        return false;
                    }
                }
            }
            foreach (IVehicle vehicle in currentVehicles)
            {
                if (vehicle is not null)
                {
                    if (compareStringsFunc.Invoke(licenseNumber, vehicle.GetVehicleInformation().LicenseNumber))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Gets the standard integer properties for a vehicle from input.
        private int[] GetRestOfProperties(Action<string> outputAction,
                                          Func<int> inputFuncInt)
        {
            int[] result = new int[3];
            outputAction(Message.InputVehicleSizeProperty);
            result[0] = inputFuncInt.Invoke();
            outputAction(Message.InputVehicleIntegerProperty("number of wheels"));
            result[1] = inputFuncInt.Invoke();
            outputAction(Message.InputVehicleIntegerProperty("number of seats"));
            result[2] = inputFuncInt.Invoke();
            return result;
        }

        // Gets fuel type from input.

        // ToDo: Reuse code from SelectByFuelType() in Manager!
        // This is copy-paste (without returning the vehicles)!
        private FuelType GetFuelType(Action<string> outputAction,
                                     Func<int> inputFuncInt)
        {
            int repeats = 0;
            do
            {
                outputAction(Environment.NewLine + Message.InputVehicleFuelType);
                int i = 1;
                foreach (FuelType fuelType in IVehicle.FuelTypes)
                {
                    outputAction($"{i}. {IVehicle.FuelTypes[i - 1]}." + Environment.NewLine);
                    i++;
                }
                outputAction("");
                int input = inputFuncInt.Invoke();
                if (input > 0 && input <= IVehicle.FuelTypes.Count())
                    return IVehicle.FuelTypes[input - 1];
                repeats++;
            } while (repeats < 100);
            throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
        }

        // Instantiates and returns a vehicle, with one overload depending on how
        // many properties to add.
        private IVehicle InstantiateVehicle(VehicleType vehicleType,
                                            string licenseNumber,
                                            Color color)
        {
            switch (vehicleType)
            {
                case VehicleType.Car:
                    return new Car(licenseNumber, color);

                case VehicleType.Airplane:
                    return new Airplane(licenseNumber, color);

                case VehicleType.Motorcycle:
                    return new Motorcycle(licenseNumber, color);

                case VehicleType.Trike:
                    return new Trike(licenseNumber, color);

                case VehicleType.Boat:
                    return new Boat(licenseNumber, color);

                case VehicleType.Bus:
                    return new Bus(licenseNumber, color);

                default:
                    return new Car(licenseNumber, color);

            }
        }

        private IVehicle InstantiateVehicle(VehicleType vehicleType,
                                            string licenseNumber,
                                            Color color,
                                            int size,
                                            int numberOfWheels,
                                            int numberOfSeats,
                                            FuelType fuelType)
        {
            switch (vehicleType)
            {
                case VehicleType.Car:
                    return new Car(licenseNumber, color, numberOfSeats, fuelType: fuelType);

                case VehicleType.Airplane:
                    return new Airplane(licenseNumber, color,numberOfWheels, numberOfSeats);

                case VehicleType.Motorcycle:
                    return new Motorcycle(licenseNumber, color, numberOfSeats);

                case VehicleType.Trike:
                    return new Trike(licenseNumber, color, numberOfSeats);

                case VehicleType.Boat:
                    return new Boat(licenseNumber, color, size, numberOfSeats);

                case VehicleType.Bus:
                    return new Bus(licenseNumber, color, size, numberOfSeats, fuelType: fuelType);

                default:
                    return new Car(licenseNumber, color);

            }
        }

        public void AddVehicleToGarage(IVehicle vehicle, int index, Action<string> outputAction)
        {
            if (vehicle is null) outputAction(Message.AddVehicleIsNull);
            else if (_garages.Get(index) is null) outputAction(Message.AddVehicleGarageDoesNotExist(index));
            else
            {
                if (_garages.Get(index)!.Add(vehicle))
                    outputAction(Message.AddVehicleSuccess(vehicle.GetVehicleInformation().LicenseNumber, index + 1));
                else
                    outputAction(Message.AddVehicleFail(index + 1));
            }
        }

        // Removes a vehicle from a garage.
        public bool RemoveVehicle(int index, string licenseNumber, Func<string, string, bool> compareStringsFunc)
        {
            IGarage<IVehicle>? garage = _garages.Get(index);
            if (garage is null) return false;
            foreach (IVehicle vehicle in garage)
            {
                if (compareStringsFunc.Invoke(licenseNumber,
                                              vehicle.GetVehicleInformation().LicenseNumber))
                {
                    garage.Remove(vehicle.GetVehicleInformation().LicenseNumber);
                    return true;
                }
            }
            return false;
        }
    }
}
