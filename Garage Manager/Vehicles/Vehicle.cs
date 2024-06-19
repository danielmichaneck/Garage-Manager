using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal abstract class Vehicle : IVehicle
    {
        protected VehicleInformation _vehicleInformation;

        protected FuelType _fuelType;
        protected int _numberOfSeats;

        public Vehicle(VehicleType vehicleType, string licenseNumber, Color color, int size, int numberOfWheels)
        {
            _vehicleInformation = new(vehicleType,
                                      licenseNumber,
                                      color,
                                      size,
                                      numberOfWheels,
                                      FuelType.Gasoline);
        }

        VehicleInformation IVehicle.GetVehicleInformation()
        {
            return _vehicleInformation;
        }
    }
}
