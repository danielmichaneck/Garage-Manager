using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class GarageList<T> : IEnumerable<T> where T : IGarage<IVehicle>
    {
        private T[] _list;
        private int _count = 0;
        public int Count => _count;

        public GarageList()
        {
            _list = new T[2];
        }

        internal void Add(T t)
        {
            // If the array is too small: double it in size.
            if (_count >= _list.Length)
            {
                T[] newList = new T[_list.Length * 2];
                for (int i = 0; i < _list.Length; i++)
                {
                    newList[i] = _list[i];
                }
                _list = newList;
            }
            for (int i = 0; i < _list.Length; i++)
            {
                if (_list[i] == null)
                {
                    _list[i] = t;
                    _count++;
                    break;
                }
            }
        }

        internal void Remove(int index)
        {
            if (index < _count)
            {
                T[] newList = new T[_list.Length];
                int place = 0;
                for (int i = 0; i < _list.Length; i++)
                {
                    if (i != index)
                    {
                        newList[place] = _list[i];
                        place++;
                    }
                }
                _list = newList;
                _count--;
            }
        }

        public T? Get(int index)
        {
            if (index < _list.Length && _list[index] is not null)
            {
                return _list[index];
            }
            else return default;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _list.Length; i++)
            {
                if (_list[i] is not null)
                {
                    yield return _list[i];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
