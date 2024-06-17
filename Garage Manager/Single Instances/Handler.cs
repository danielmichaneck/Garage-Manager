using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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
            IGarage<IVehicle> garage = _garages.Get(index) ?? new Garage<IVehicle>(0);
            return ListVehicleInformation(garage);
        }

        public string ListAllVehiclesInAllGarages()
        {
            StringBuilder listStringBuilder = new("");
            foreach (IGarage<IVehicle> garage in _garages)
            {
                listStringBuilder.Append(ListVehicleInformation(garage).Trim());
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

        public List<IGarage<IVehicle>> GetAllGarages()
        {
            return new List<IGarage<IVehicle>>(_garages);
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
            int numberOfVehicles = 0;
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

            switch(vehicleType)
            {
                default:
                    return new Car(licenseNumber, color);

            }
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
                for (int p = 0; p < IVehicle.VehicleTypes.Length; p++)
                {
                    if (compareStringsFunc.Invoke(input, IVehicle.VehicleTypes[p].ToString()))
                    {
                        return IVehicle.VehicleTypes[p];
                    }
                }
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
                foreach (Garage<IVehicle> garage in _garages)
                {
                    foreach (IVehicle vehicle in garage)
                    {
                        if (compareStringsFunc.Invoke(licenseNumber, vehicle.GetVehicleInformation()._licenseNumber))
                        {
                            unique = false;
                            outputAction.Invoke(Message.InputVehicleLicenseNumberNotUnique);
                        }
                    }
                }
                foreach (IVehicle vehicle in currentVehicles)
                {
                    if (vehicle is not null)
                    {
                        if (compareStringsFunc.Invoke(licenseNumber, vehicle.GetVehicleInformation()._licenseNumber))
                        {
                            unique = false;
                            outputAction.Invoke(Message.InputVehicleLicenseNumberNotUnique);
                        }
                    }
                }
                repeats++;
            } while (!unique && repeats < 100);
            if (repeats >= 100) throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
            return licenseNumber;
        }

        private Color GetColor(Action<string> outputAction,
                               Func<string> inputFunc,
                               Func<string, string, bool> compareStringsFunc)
        {
            Color color;
            int repeats = 0;
            do
            {
                outputAction.Invoke(Message.InputVehicleColor);
                color = Color.FromName(inputFunc.Invoke());
            } while (!color.IsKnownColor && repeats < 100);
            if (repeats >= 100) throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
            return color;
        }
    }
}
