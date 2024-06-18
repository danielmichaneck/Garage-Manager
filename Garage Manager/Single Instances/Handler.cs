using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Handler Tests")]

namespace Garage_Manager
{
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

        public IGarage<IVehicle> CreateNewGarage(int size)
        {
            var newGarage = CreateNewGarage(size, Array.Empty<IVehicle>());
            AddGarage(newGarage);
            return newGarage;
        }

        private string ListVehicleInformation(IGarage<IVehicle> garage)
        {
            StringBuilder listStringBuilder = new("");
            foreach (IVehicle vehicle in garage)
            {
                listStringBuilder.Append(vehicle.GetVehicleInformation().ToString() + Environment.NewLine
                                                                                    + Environment.NewLine);
            }
            return listStringBuilder.ToString().Trim();
        }

        public string ListAllVehiclesInGarage(int index)
        {
            IGarage<IVehicle>? garage = _garages.Get(index);
            if (garage is null) return Message.ListSpecificGarageDoesNotExist;
            else if (garage.Count() < 1) return Message.ListEmptyGarage;
            return ListVehicleInformation(garage);
        }

        public string ListAllVehiclesInAllGarages()
        {
            StringBuilder listStringBuilder = new("");
            int i = 0;
            foreach (IGarage<IVehicle> garage in _garages)
            {
                listStringBuilder.Append(Environment.NewLine + Environment.NewLine + $"Garage {++i}:" + Environment.NewLine);
                if (garage.Count() < 1) listStringBuilder.Append(Message.ListEmptyGarage + Environment.NewLine);
                listStringBuilder.Append(ListVehicleInformation(garage).Trim() + Environment.NewLine);
            }
            return listStringBuilder.ToString().Trim();
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
                int numberOfVehicles = PopulateGarage(size, outputAction, inputFunc, inputFuncInt, compareStringsFunc);
                outputAction.Invoke(Message.GarageCreated(size, numberOfVehicles) + Environment.NewLine);
            }
            // Creates garage without populating it.
            else outputAction.Invoke(Message.GarageCreated(size) + Environment.NewLine);
        }

        private int PopulateGarage(int size, Action<string> outputAction,
                                    Func<string> inputFunc,
                                    Func<int> inputFuncInt,
                                    Func<string, string, bool> compareStringsFunc)
        {
            outputAction.Invoke(Message.NumberOfVehicles);
            int numberOfVehicles;
            do
            {
                numberOfVehicles = inputFuncInt.Invoke();
            } while (numberOfVehicles > size);
            IVehicle[] newVehicles = new IVehicle[numberOfVehicles];
            for (int i = 0; i < numberOfVehicles; i++)
            {
                newVehicles[i] = CreateVehicle(newVehicles,
                                               outputAction,
                                               inputFunc,
                                               compareStringsFunc);
            }
            CreateNewGarage(size, newVehicles);
            return numberOfVehicles;
        }

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

        private Color? GetColor(string colorAsString)
        {
            Color color = Color.FromName(colorAsString);
            if (!color.IsKnownColor) return null;
            return color;
        }

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
                if (color is null) outputAction.Invoke(Message.InputVehicleColorNotRecognized());
                repeats++;
                if (repeats >= 100) throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
            } while (color is null);
            return (Color)color!;
        }

        private bool CheckLicenseNumber(IVehicle[] currentVehicles,
                                        string licenseNumber,
                                        Func<string, string, bool> compareStringsFunc)
        {
            foreach (Garage<IVehicle> garage in _garages)
            {
                foreach (IVehicle vehicle in garage)
                {
                    if (compareStringsFunc.Invoke(licenseNumber, vehicle.GetVehicleInformation()._licenseNumber))
                    {
                        return false;
                    }
                }
            }
            foreach (IVehicle vehicle in currentVehicles)
            {
                if (vehicle is not null)
                {
                    if (compareStringsFunc.Invoke(licenseNumber, vehicle.GetVehicleInformation()._licenseNumber))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private IVehicle InstantiateVehicle(VehicleType vehicleType,
                                            string licenseNumber,
                                            Color color)
        {
            switch (vehicleType)
            {
                default:
                    return new Car(licenseNumber, color);

            }
        }
    }
}
