using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal interface IHandler
    {
        internal void AddGarage(IGarage<IVehicle> garage);
    }
}
