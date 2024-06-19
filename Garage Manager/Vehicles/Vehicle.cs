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

        public Vehicle(VehicleType vehicleType,
                       string licenseNumber,
                       Color color,
                       int size,
                       int numberOfWheels,
                       int numberOfSeats = 5,
                       FuelType fuelType = FuelType.Gasoline)
        {
            _vehicleInformation = new(vehicleType,
                                      licenseNumber,
                                      color,
                                      size,
                                      numberOfWheels,
                                      numberOfSeats,
                                      fuelType);
        }

        VehicleInformation IVehicle.GetVehicleInformation()
        {
            return _vehicleInformation;
        }
    }
}
