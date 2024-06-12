using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Garage<T> where T : IVehicleCanPark, IGarage
    {
        private T[] _vehicleList;

        public Garage()
        {
            
        }
    }
}
