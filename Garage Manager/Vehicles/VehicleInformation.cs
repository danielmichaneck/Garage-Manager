using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    public struct VehicleInformation
    {
        public readonly VehicleType Vehicletype;
        public readonly string LicenseNumber;
        public readonly Color Color;
        public readonly int Size;
        public readonly int NumberOfWheels;
        public readonly int NumberOfSeats;
        public readonly FuelType FuelType;
        private string[]? _additionalProperties;

        public VehicleInformation(VehicleType vehicleType,
                                  string licenseNumber,
                                  Color color,
                                  int size,
                                  int numberOfWheels,
                                  int numberOfSeats,
                                  FuelType fuelType)
        {
            Vehicletype = vehicleType;
            LicenseNumber = licenseNumber;
            Color = color;
            Size = size;
            NumberOfWheels = numberOfWheels;
            NumberOfSeats = numberOfSeats;
            FuelType = fuelType;
        }

        public void SetAdditionalProperties(string additionalProperty)
        {
            _additionalProperties = new string[1];
            _additionalProperties[0] = additionalProperty;
        }

        public void SetAdditionalProperties(string[] additionalProperties)
        {
            _additionalProperties = new string[additionalProperties.Length];
            for (int i = 0; i < additionalProperties.Length; i++)
            {
                _additionalProperties[i] = additionalProperties[i];
            }
        }

        public string[]? GetAdditionalProperties() => _additionalProperties;

        public override string ToString()
        {
            var result = new StringBuilder(new string(
                $"Vehicle type:  {Vehicletype}" + Environment.NewLine +
                $"License number: {LicenseNumber} " + Environment.NewLine + 
                $"Color: {Color}" + Environment.NewLine +
                $"Length in meters: {Size}" + Environment.NewLine +
                $"Number of wheels: {NumberOfWheels}" + Environment.NewLine +
                $"Number of seats: {NumberOfSeats}" + Environment.NewLine +
                $"Fuel type: {FuelType}" + Environment.NewLine));
            if (_additionalProperties is not null)
            {
                for (int i = 0; i < _additionalProperties.Length; i++)
                {
                    result.Append(_additionalProperties[i] + Environment.NewLine);
                }
            }
            return result.ToString().Trim();
        }
    }
}
