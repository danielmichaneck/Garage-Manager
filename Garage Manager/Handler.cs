using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Handler : IHandler
    {
        private IGarage<IVehicle> _garage;

        public Handler()
        {
            _garage = new Garage<IVehicle>(10);
            _garage.Add(new Car("ABC 123", Color.Green));
            _garage.Add(new Car("ABC 245", Color.Red));
            _garage.Add(new Car("ABC 517", Color.RebeccaPurple));
            _garage.Add(new Car("ABC 117", Color.MintCream));

            foreach (IVehicle vehicle in _garage)
            {
                Console.WriteLine(vehicle.GetVehicleInformation().ToString());
            }
        }
    }
}
