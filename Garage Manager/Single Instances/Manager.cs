using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Garage Manager Tests")]

namespace Garage_Manager
{
    internal class Manager
    {
        private IUI _userInterface;
        private IHandler _handler;

        public Manager()
        {
            _userInterface = new UI(outputMethod: Console.WriteLine,
                                    inputMethod: Console.ReadLine);
        }

        public void RunApplication()
        {
            bool exit = false;

            do
            {


            } while (!exit);
        }

        internal IGarage<IVehicle> CreateNewGarage(IVehicle car1, IVehicle car2)
        {
            return _handler.CreateNewGarage(10);
        }
    }
}
