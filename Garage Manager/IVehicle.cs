using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal interface IVehicle
    {
        bool CompareTo<T>(T t) where T : IVehicle;
        internal VehicleInformation GetVehicleInformation();
    }
}
