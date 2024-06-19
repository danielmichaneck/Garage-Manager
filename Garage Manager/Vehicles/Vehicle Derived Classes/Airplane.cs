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

        public Airplane(string licensenumber, Color color, int numberOfSeats = 20, int size = 60, int rangeInKilometres = 5000) :
            base(VehicleType.Airplane, licensenumber, color, size, numberOfWheels: 3, numberOfSeats: numberOfSeats, fuelType: FuelType.Kerosene)
        {
            // Setting additional properties.
            _rangeInKilometres = rangeInKilometres;
            string[] properties = [$"Range in kilometres: {_rangeInKilometres}"];
            _vehicleInformation.SetAdditionalProperties(properties);
        }
    }
}
