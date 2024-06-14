using System.Security.Cryptography.X509Certificates;

namespace Garage_Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Garage manager application.";

            Manager GarageManager = new();

            GarageManager.RunApplication();
        }
    }
}
