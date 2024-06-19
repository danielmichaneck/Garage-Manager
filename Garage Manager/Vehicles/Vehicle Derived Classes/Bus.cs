using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Bus : Vehicle
    {
        private bool _freeBusFare;

        public Bus(string licensenumber, Color color, int size = 12, int numberOfWheels = 4, int numberOfSeats = 20, FuelType fuelType = FuelType.Gasoline, bool freeBusFare = true) :
            base(VehicleType.Bus, licensenumber, color, size: size, numberOfWheels, numberOfSeats, fuelType)
        {
            // Additional properties
            _freeBusFare = freeBusFare;
            string[] properties = [$"Free bus fare: {_freeBusFare}"];
            _vehicleInformation.SetAdditionalProperties(properties);
        }
    }
}
