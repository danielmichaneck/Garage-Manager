using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal interface IHandler
    {
        IGarage<IVehicle> CreateNewGarage(int size, IVehicle[] cars);
        IGarage<IVehicle> CreateNewGarage(int size);
        public void AddGarage(IGarage<IVehicle> garage);
        public IGarage<IVehicle>? GetGarage(int index);
        public GarageList<IGarage<IVehicle>> GetAllGarages();
        public int Count();
        public string ListAllVehiclesInGarage(int index);
        public string ListAllVehiclesInAllGarages();
        public void CreateGarage(Action<string> outputAction,
                                  Func<string> inputFunc,
                                  Func<int> inputFuncInt,
                                  Func<bool> inputFuncBool,
                                  Func<string, string, bool> compareStringsFunc);
        public IVehicle? CreateVehicle(IVehicle[] currentVehicles,
                                       string vehicleTypeAsString,
                                       string licenseNumber,
                                       string colorAsString,
                                       Func<string, string, bool> compareStringsFunc);
    }
}
