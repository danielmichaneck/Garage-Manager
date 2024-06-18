using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Airplane : Vehicle
    {
        private int _rangeInKilometres;

        public Airplane(string licensenumber, Color color, int numberOfSeats, int size, int rangeInKilometres) : base(VehicleType.Airplane, licensenumber, color, size, 3)
        {
            // Setting additional properties.
            _numberOfSeats = numberOfSeats;
            _rangeInKilometres = rangeInKilometres;
            _fuelType = FuelType.Kerosene;
            string[] properties = [$"Number of seats: {_numberOfSeats}",
                                   $"Range in kilometres: {_rangeInKilometres}",
                                   $"Fuel type: {_fuelType}"];
            _vehicleInformation.SetAdditionalProperties(properties);
        }

        public Airplane(string licensenumber, Color color) : base(VehicleType.Airplane, licensenumber, color, 30, 3)
        {
            // Setting additional properties.
            _numberOfSeats = 60;
            _rangeInKilometres = 5000;
            _fuelType = FuelType.Kerosene;
            string[] properties = [$"Number of seats: {_numberOfSeats}",
                                   $"Range in kilometres: {_rangeInKilometres}",
                                   $"Fuel type: {_fuelType}"];
            _vehicleInformation.SetAdditionalProperties(properties);
        }
    }
}
