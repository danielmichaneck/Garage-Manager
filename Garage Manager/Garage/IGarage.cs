using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Garage Manager Tests")]

namespace Garage_Manager
{
    internal interface IGarage<T> : IEnumerable<T>
    {
        internal bool Add(T add);
        internal bool Remove(string licenseNumber);
        public string GetAllVehicleInformation();
    }
}
