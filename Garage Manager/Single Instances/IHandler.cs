using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal interface IHandler
    {
        IGarage<IVehicle> CreateNewGarage(int size, List<IVehicle> cars);
        IGarage<IVehicle> CreateNewGarage(int size);
        internal void AddGarage(IGarage<IVehicle> garage);
    }
}
