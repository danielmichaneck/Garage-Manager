using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal interface IUI
    {
        string? GetInput(Func<string?> _inputMethod);
        string? GetInput();
        string GetValidInput(Func<string?> _inputMethod);
        string GetValidInput();
        void PrintMessage(string message, Action<string> printMethod);
        void PrintMessage(string message);
    }
}
