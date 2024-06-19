using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Boat : Vehicle
    {
        private int _speedInKnots;

        public Boat(string licensenumber, Color color, int numberOfSeats = 60, int size = 60, int speedInKnots = 20) :
            base(VehicleType.Boat, licensenumber, color, size, numberOfWheels: 0, numberOfSeats: numberOfSeats, fuelType: FuelType.Diesel)
        {
            // Setting additional properties.
            _speedInKnots = speedInKnots;
            string[] properties = [$"Speed in knots: {_speedInKnots}"];
            _vehicleInformation.SetAdditionalProperties(properties);
        }
    }
}
