using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal interface IVehicle
    {
        public static VehicleType[] VehicleTypes = [VehicleType.Car, VehicleType.Airplane];
        public static VehicleType? GetVehicleType(string vehicleTypeAsString,
                                           Func<string, string, bool> compareStringsFunc)
        {
            for (int p = 0; p < VehicleTypes.Length; p++)
            {
                if (compareStringsFunc.Invoke(vehicleTypeAsString, VehicleTypes[p].ToString()))
                {
                    return VehicleTypes[p];
                }
            }
            return null;
        }
        internal VehicleInformation GetVehicleInformation();
    }
}
