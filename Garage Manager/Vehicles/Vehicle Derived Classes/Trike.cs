using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Trike : Vehicle
    {
        private int _numberOfGears;
        private int _rimSizeInInches;

        public Trike(string licensenumber, Color color, int numberOfSeats = 1, int numberOfGears = 5, int rimSizeInInches = 20) :
            base(VehicleType.Trike, licensenumber, color, size: 1, numberOfWheels: 3, numberOfSeats: numberOfSeats, fuelType: FuelType.Gasoline)
        {
            // Additional properties
            _numberOfGears = numberOfGears;
            _rimSizeInInches = rimSizeInInches;
            string[] properties = [$"Number of gears: {_numberOfGears}" + Environment.NewLine +
                                   $"Rim size in inches: {_rimSizeInInches}"];
            _vehicleInformation.SetAdditionalProperties(properties);
        }
    }
}
