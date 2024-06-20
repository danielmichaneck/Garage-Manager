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

        public Car(string licensenumber, Color color, int numberOfSeats = 5, int numberOfGears = 5, FuelType fuelType = FuelType.Gasoline) :
            base(VehicleType.Car, licensenumber, color, size: 2, numberOfWheels: 4, numberOfSeats: numberOfSeats, fuelType: fuelType)
        {
            // Additional properties
            _numberOfGears = numberOfGears;
            string[] properties = [$"Number of gears: {_numberOfGears}"];
            _vehicleInformation.SetAdditionalProperties(properties);
        }
    }
}
