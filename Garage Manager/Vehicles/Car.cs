using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Car : Vehicle
    {
        public Car(string licensenumber, Color color) : base(licensenumber, color, 4, 4)
        {
            _vehicleInformation.SetAdditionalProperties("Number of seats: 5");
        }
    }
}
