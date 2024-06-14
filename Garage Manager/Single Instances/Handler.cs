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

        public void AddGarage(IGarage<IVehicle> garage)
        {
            _garages.Add(garage);
        }

        public IGarage<IVehicle> CreateNewGarage(int size, List<IVehicle> cars)
        {
            IGarage<IVehicle> newGarage = new Garage<IVehicle>(size);
            for (int i = 0; i < cars.Count; i++)
            {
                newGarage.Add(cars[i]);
            }
            AddGarage(newGarage);
            return newGarage;
        }

        public IGarage<IVehicle> CreateNewGarage(int size)
        {
            var newGarage = CreateNewGarage(size, new List<IVehicle>());
            AddGarage(newGarage);
            return newGarage;
        }

        public string ListAllVehiclesInGarage(int index)
        {
            StringBuilder listStringBuilder = new("");
            IGarage<IVehicle> garage = _garages.Get(index) ?? new Garage<IVehicle>(0);
            foreach (IVehicle vehicle in garage)
            {
                listStringBuilder.Append(vehicle.GetVehicleInformation().ToString() + Environment.NewLine);
            }
            return listStringBuilder.ToString().Trim();
        }

        public string ListAllVehiclesInAllGarages()
        {
            StringBuilder listStringBuilder = new("");
            foreach (IGarage<IVehicle> garage in _garages)
            {
                foreach (IVehicle vehicle in garage)
                {
                    listStringBuilder.Append(vehicle.GetVehicleInformation().ToString() + Environment.NewLine);
                }
            }
            return listStringBuilder.ToString().Trim();
        }
    }
}
