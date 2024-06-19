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

        public Car(string licensenumber, Color color, int numberOfSeats = 5, int numberOfGears = 5) :
            base(VehicleType.Car, licensenumber, color, 1, 4, fuelType: FuelType.Gasoline)
        {
            // Additional properties
            _numberOfGears = numberOfGears;
            string[] properties = [$"Number of gears: {_numberOfGears}"];
            _vehicleInformation.SetAdditionalProperties(properties);
        }
    }
}
