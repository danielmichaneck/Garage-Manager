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
            "2. Create a new garage." + Environment.NewLine +
            "3. List all vehicles in all garages.";
        public static string CreateGarageSize => "Please enter how many parking spots you would like in the garage.";
        public static string PopulateGarage => "Would you like to populate the garage from the start?";
        public static string NumberOfVehicles => "How many vehicles would you like to add to the garage?";
        public static string GarageCreated(int spots) => $"You have created a garage with {spots} parking spots.";
        public static string GarageCreated(int spots, int vehicles) => $"You have created a garage with {spots} parking spots and {vehicles} vehicles in it.";

        // Populate garage
        public static string InputVehicleType => "Please enter the vehicle type.";
        public static string InputVehicleTypeNotFound(string vehicleType) => $"No vehicle type named {vehicleType} was found.";
        public static string InputVehicleLicenseNumber => "Please enter the vehicle's unique license number.";
        public static string InputVehicleLicenseNumberNotUnique => "Another vehicle already has that license number.";
        public static string InputVehicleColor => "Please enter the name of the vehicle's color.";

        // UI Input
        public static string InputValidBool => "Please input input yes to accept or no to reject.";
        public static string InputValidInt => "Please input an integer value.";
        public static string InputNotValid => "Your input was not recognized. Please try again.";

        // Error messages
        public static string ErrorNoValidInputIn100Tries => "No valid input given in 100 tries.";
    }
}
