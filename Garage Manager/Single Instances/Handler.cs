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
        private GarageList<IGarage<IVehicle>> _garages;

        public Handler()
        {
            _garages = new();
        }

        public IGarage<IVehicle> CreateNewGarage(int size, List<IVehicle> cars)
        {
            IGarage<IVehicle> newGarage = new Garage<IVehicle>(size);
            for (int i = 0; i < cars.Count; i++)
            {
                newGarage.Add(cars[i]);
            }
            _garages.Add(newGarage);
            return newGarage;
        }

        public IGarage<IVehicle> CreateNewGarage(int size)
        {
            var newGarage = CreateNewGarage(size, new List<IVehicle>());
            _garages.Add(newGarage);
            return newGarage;
        }

        void IHandler.AddGarage(IGarage<IVehicle> garage)
        {
            throw new NotImplementedException();
        }
    }
}
