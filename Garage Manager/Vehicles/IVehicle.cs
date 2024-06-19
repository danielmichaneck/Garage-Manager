using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal interface IVehicle
    {
        public static FuelType[] FuelTypes = [FuelType.Gasoline, FuelType.Diesel, FuelType.Kerosene];

        public static VehicleType[] VehicleTypes = [VehicleType.Car, VehicleType.Airplane, VehicleType.Motorcycle, VehicleType.Trike];

        public static VehicleType? GetVehicleType(string vehicleTypeAsString,
                                           Func<string, string, bool> compareStringsFunc)
        {
            for (int i = 0; i < VehicleTypes.Length; i++)
            {
                if (compareStringsFunc.Invoke(vehicleTypeAsString, VehicleTypes[i].ToString()))
                {
                    return VehicleTypes[i];
                }
            }
            return null;
        }

        internal VehicleInformation GetVehicleInformation();
    }
}
