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

        public string GetValidInput(Func<string?> inputMethod)
        {
            string result;
            int excepctionCounter = 0;
            do
            {
                result = GetInput(inputMethod) ?? "";
                if (excepctionCounter > 100) throw new InvalidOperationException("No valid input given in 100 tries.");
                excepctionCounter++;
            }
            while (String.IsNullOrWhiteSpace(result));
            return result;
        }

        public string GetValidInput()
        {
            return GetValidInput(_inputMethod);
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
