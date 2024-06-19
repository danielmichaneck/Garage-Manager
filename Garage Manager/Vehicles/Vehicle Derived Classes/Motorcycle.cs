using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Motorcycle : Vehicle
    {
        private int _numberOfGears;
        private int _rimSizeInInches;

        public Motorcycle(string licensenumber, Color color, int numberOfSeats = 1, int numberOfGears = 5, int rimSizeInInches = 20) :
            base(VehicleType.Motorcycle, licensenumber, color, 1, 4, numberOfSeats: numberOfSeats, fuelType: FuelType.Gasoline)
        {
            // Additional properties
            _numberOfGears = numberOfGears;
            _rimSizeInInches = rimSizeInInches;
            string[] properties = [$"Number of gears: {_numberOfGears}" +
                                   $"Rim size in inches: {_rimSizeInInches}"];
            _vehicleInformation.SetAdditionalProperties(properties);
        }
    }
}
