using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    internal interface IUI
    {
        string? GetInput(Func<string?> inputMethod);
        string? GetInput();
        string GetValidInput(Func<string?> inputMethod);
        string GetValidInput();
        bool GetValidBool(Func<string?> inputMethod);
        bool GetValidBool();
        int GetValidInt(Func<string?> inputMethod);
        int GetValidInt();
        bool CheckIfSameString(string first, string second);
        void PrintMessage(string message, Action<string> printMethod);
        void PrintMessage(string message);
        void ClearOutput();
    }
}
