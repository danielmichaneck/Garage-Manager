using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Manager
    {
        private IHandler _handler;

        public void ManageGarage()
        {
            _handler = new Handler();


        }
    }
}
