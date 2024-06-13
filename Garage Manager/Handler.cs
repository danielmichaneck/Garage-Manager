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
            _garage = new Garage<IVehicle>(19);
        }
    }
}
