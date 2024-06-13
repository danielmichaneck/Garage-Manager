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
        private int _count = -1;

        public GarageList()
        {
            _list = new T[2];
        }

        internal void Add(T t)
        {
            if (_count >= _list.Length -1)
            {
                T[] newList = new T[_list.Length * 2];
                for (int i = 0; i < _list.Length; i++)
                {
                    newList[i] = _list[i];
                }
            }
            _list[++_count] = t;
        }

        internal void Remove(int index)
        {
            if (index <= _count)
            {
                T[] newList = new T[_list.Length];
                int place = 0;
                for (int i = 0; i < _list.Length; i++)
                {
                    if (i != index)
                    {
                        newList[place] = _list[i];
                    }
                    place++;
                }
                _list = newList;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _list[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
