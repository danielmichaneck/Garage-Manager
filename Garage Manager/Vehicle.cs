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
        protected int _numberOfEngines;
        protected int _cylinderVolume;
        protected int _numberOfSeats;
        protected int _length;

        public Vehicle(string licenseNumber, Color color, int quarterSize, int numberOfWheels)
        {
            _vehicleInformation = new(licenseNumber,
                                      color,
                                      quarterSize,
                                      numberOfWheels);
        }

        VehicleInformation IVehicle.GetVehicleInformation()
        {
            return _vehicleInformation;
        }
    }
}
