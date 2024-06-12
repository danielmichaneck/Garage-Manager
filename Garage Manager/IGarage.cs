using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal interface IGarage<T>
    {
        internal bool Add(T add);
        internal bool Remove(T remove);
    }
}
