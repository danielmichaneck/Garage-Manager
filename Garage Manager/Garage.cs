using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Garage<T> : IGarage<T>, IEnumerable<T> where T : IVehicle
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

        bool IGarage<T>.Remove(T remove)
        {
            for (int i = 0; i < _list.Length; i++)
            {
                if (_occupied[i] && remove.CompareTo(_list[i]))
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
                yield return _list[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return _list;
        }
    }
}
