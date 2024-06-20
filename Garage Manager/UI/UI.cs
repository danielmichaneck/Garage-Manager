using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    /// <summary>
    /// A user interface that can hold an outputmethod, inputmethod, and clearmethod.
    /// Can get valid strings, ints, and bools.
    /// Can compare two strings to see if they contain the same letters, whether
    /// uppercase or lowercase.
    /// 
    /// The program will throw an Exception if a valid input has not been given
    /// in 100 tries.
    /// </summary>
    internal class UI : IUI
    {
        private Action<string> _outputMethod;
        private Func<string?> _inputMethod;
        private Action _clearMethod;

        public UI(Action<string> outputMethod, Func<string?> inputMethod, Action clearMethod)
        {
            _outputMethod = outputMethod;
            _inputMethod = inputMethod;
            _clearMethod = clearMethod;
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
                if (excepctionCounter > 0) PrintMessage(Message.InputNotValid);
                result = GetInput(inputMethod) ?? "";
                if (excepctionCounter > 100) throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
                if (result is null) PrintMessage(Message.InputNotValid);
                excepctionCounter++;
            }
            while (String.IsNullOrWhiteSpace(result));
            return result;
        }

        public string GetValidInput()
        {
            return GetValidInput(_inputMethod);
        }

        public bool GetValidBool(Func<string?> inputMethod)
        {
            bool result = false;
            int repeats = 0;
            string[] acceptInput = ["y", "ye", "yes"];
            string[] rejectInput = ["n", "no"];
            string input;
            do
            {
                PrintMessage(Message.InputValidBool);
                input = GetValidInput(inputMethod);
                if (ContainsString(input, acceptInput))
                {
                    result = true;
                    break;
                }
                else if (ContainsString(input, rejectInput))
                {
                    result = false;
                    break;
                }
                repeats++;
                if (repeats > 100) throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
                PrintMessage(Message.InputNotValid);
            } while (true);
            return result;
        }

        public bool GetValidBool()
        {
            return GetValidBool(_inputMethod);
        }

        public int GetValidInt(Func<string?> inputMethod)
        {
            int result;
            int repeats = 0;
            string input;
            do
            {
                PrintMessage(Message.InputValidInt);
                input = GetValidInput(inputMethod);
                if (int.TryParse(input, out result)) break;
                repeats++;
                if (repeats > 100) throw new InvalidOperationException(Message.ErrorNoValidInputIn100Tries);
                PrintMessage(Message.InputNotValid);
            } while (true);
            return result;
        }

        public int GetValidInt()
        {
            return GetValidInt(_inputMethod);
        }

        private bool ContainsString(string first, string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (CheckIfSameString(first, array[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the string arguments contain the same chars
        /// whether the letters are lowercase or uppercase.
        /// Should trim strings before inputing them here!
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public bool CheckIfSameString(string first, string second)
        {
            if (first.Length != second.Length) return false;
            first = first.ToLower();
            second = second.ToLower();
            for(int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i]) return false;
            }
            return true;
        }

        public void PrintMessage(string message, Action<string> printMethod)
        {
            printMethod.Invoke(message);
        }

        public void PrintMessage(string message)
        {
            PrintMessage(message, _outputMethod);
        }

        public void ClearOutput()
        {
            _clearMethod.Invoke();
            PrintMessage("___________________________________________________");
        }
    }
}
