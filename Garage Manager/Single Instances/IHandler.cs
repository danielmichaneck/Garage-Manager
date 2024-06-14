using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal interface IHandler
    {
        IGarage<IVehicle> CreateNewGarage(int size, List<IVehicle> cars);
        IGarage<IVehicle> CreateNewGarage(int size);
        public void AddGarage(IGarage<IVehicle> garage);
        public IGarage<IVehicle>? GetGarage(int index);
        public List<IGarage<IVehicle>> GetAllGarages();
        public string ListAllVehiclesInGarage(int index);
        public string ListAllVehiclesInAllGarages();
    }
}
