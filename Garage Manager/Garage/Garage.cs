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
    /// <summary>
    /// IEnumerable<T> that stores elements in an array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Garage<T> : IGarage<T> where T : IVehicle
    {
        private T[] _list;
        public T[] ToArray() => _list;

        private bool[] _occupied;

        public Garage(int size)
        {
            _list = new T[size];
            _occupied = new bool[size];
        }

        public Garage(int size, T[] initList)
        {
            _list = new T[size];
            _occupied = new bool[initList.Length];
            for (int i = 0; i < initList.Length; i++)
            {
                _list[i] = initList[i];
                _occupied[i] = true;
            }
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
            for (int i = 0; i < _list.Length; i++)
            {
                if (_occupied[i] && _list[i].GetVehicleInformation().LicenseNumber == licenseNumber)
                {
                    _occupied[i] = false;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the number of elements stored in the array
        /// rather than the length of the array itself.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            int result = 0;
            for (int i = 0; i < _list.Length; i++)
            {
                if (_occupied[i]) result++;
            }
            return result;
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
    }
}
