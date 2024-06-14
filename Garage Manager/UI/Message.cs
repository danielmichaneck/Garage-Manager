using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    public struct Message
    {
        public static string Start => "Please select an option from the menu." + Environment.NewLine +
            "0. Exit the application." + Environment.NewLine +
            "1. Read data from a file." + Environment.NewLine +
            "2. Create a new garage.";
        public static string CreateGarageSize => "Please enter how many parking spots you would like in the garage.";
        public static string PopulateGarage => "Would you like to populate the garage from the start?";
        public static string GarageCreated(int spots) => $"You have created a garage with {spots} parking spots.";
        public static string GarageCreated(int spots, int vehicles) => $"You have created a garage with {spots} parking spots and {vehicles} vehicles in it.";
    }
}
