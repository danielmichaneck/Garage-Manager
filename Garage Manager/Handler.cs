using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Handler : IHandler
    {
        private Garage<IVehicle> _garage;

        public Handler()
        {
            _garage = new(10);

            foreach (IVehicle vehicle in _garage)
            {
                Console.WriteLine(vehicle.GetVehicleInformation().ToString());
            }
        }
    }
}
