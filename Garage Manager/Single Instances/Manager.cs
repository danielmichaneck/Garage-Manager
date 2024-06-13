using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class Manager
    {
        private IUI _userInterface;

        public Manager()
        {
            _userInterface = new UI(outputMethod: Console.WriteLine,
                                    inputMethod: Console.ReadLine);
        }

        public void RunApplication()
        {
            bool exit = false;

            do
            {


            } while (!exit);
        }
    }
}
