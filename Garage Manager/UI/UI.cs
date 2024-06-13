using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal class UI : IUI
    {
        private Action<string> _outputMethod;
        private Func<string?> _inputMethod;

        public UI(Action<string> outputMethod, Func<string?> inputMethod)
        {
            _outputMethod = outputMethod;
            _inputMethod = inputMethod;
        }

        public string? GetInput(Func<string?> inputMethod)
        {
            return inputMethod.Invoke();
        }

        public string? GetInput()
        {
            return GetInput(_inputMethod);
        }

        public void PrintMessage(string message, Action<string> printMethod)
        {
            printMethod.Invoke(message);
        }

        public void PrintMessage(string message)
        {
            PrintMessage(message, _outputMethod);
        }
    }
}
