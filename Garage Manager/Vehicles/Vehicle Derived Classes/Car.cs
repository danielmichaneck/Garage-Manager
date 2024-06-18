using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Car : Vehicle
    {
        private int _numberOfGears;

        public Car(string licensenumber, Color color, int numberOfSeats, int numberOfGears) : base(VehicleType.Car, licensenumber, color, 1, 4)
        {
            // Additional properties
            _numberOfSeats = numberOfSeats;
            _numberOfGears = numberOfGears;
            _fuelType = FuelType.Gasoline;
            string[] properties = [$"Number of seats: {_numberOfSeats}",
                                    $"Number of gears: {_numberOfGears}",
                                   $"Fuel type: {_fuelType}"];
            _vehicleInformation.SetAdditionalProperties(properties);
        }

        public Car(string licensenumber, Color color) : base(VehicleType.Car, licensenumber, color, 1, 4)
        {
            // Additional properties
            _numberOfSeats = 5;
            _numberOfGears = 5;
            _fuelType = FuelType.Gasoline;
            string[] properties = [$"Number of seats: {_numberOfSeats}",
                                    $"Number of gears: {_numberOfGears}",
                                   $"Fuel type: {_fuelType}"];
            _vehicleInformation.SetAdditionalProperties(properties);
        }
    }
}
