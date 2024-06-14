using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Garage Manager Tests")]

namespace Garage_Manager
{
    internal class Garage<T> : IGarage<T> where T : IVehicle
    {
        private T[] _list;
        private bool[] _occupied;

        public Garage(int size)
        {
            _list = new T[size];
            _occupied = new bool[size];
        }

        bool IGarage<T>.Add(T add)
        {
            for (int i = 0; i < _occupied.Length; i++)
            {
                if (!_occupied[i])
                {
                    _list[i] = add;
                    _occupied[i] = true;
                    return true;
                }
            }
            return false;
        }

        bool IGarage<T>.Remove(string licenseNumber)
        {
            for (int i = 0; i < _occupied.Length; i++)
            {
                if (_occupied[i] && _list[i].GetVehicleInformation()._licenseNumber == licenseNumber)
                {
                    _occupied[i] = false;
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _list.Length; i++)
            {
                if (_occupied[i]) yield return _list[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string GetAllVehicleInformation()
        {
            throw new NotImplementedException();
        }
    }
}
