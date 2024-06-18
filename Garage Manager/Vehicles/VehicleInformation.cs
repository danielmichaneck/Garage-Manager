﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    public struct VehicleInformation
    {
        public readonly VehicleType _vehicletype;
        public readonly string _licenseNumber;
        public readonly Color _color;
        public readonly int _size;
        public readonly int _numberOfWheels;
        private string[]? _additionalProperties;

        public VehicleInformation(VehicleType vehicleType, string licenseNumber, Color color, int size, int numberOfWheels)
        {
            _vehicletype = vehicleType;
            _licenseNumber = licenseNumber;
            _color = color;
            _size = size;
            _numberOfWheels = numberOfWheels;
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
                $"Vehicle type:  {_vehicletype}" + Environment.NewLine +
                $"License number: {_licenseNumber} " + Environment.NewLine + 
                $"Color: {_color}" + Environment.NewLine +
                $"Size as number of parking spaces it requires: {_size}" + Environment.NewLine +
                $"Number of wheels: {_numberOfWheels}" + Environment.NewLine));
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
