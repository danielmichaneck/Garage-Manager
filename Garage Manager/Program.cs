using System.Security.Cryptography.X509Certificates;

namespace Garage_Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Manager GarageManager = new();

            GarageManager.ManageGarage();
        }
    }
}
