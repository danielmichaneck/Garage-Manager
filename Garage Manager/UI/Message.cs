using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    public struct Message
    {
        // Main menu
        public static string Start => "Please select an option from the menu." + Environment.NewLine +
            "0. Exit the application." + Environment.NewLine +
            "1. Read data from a file." + Environment.NewLine +
            "2. List all garages." + Environment.NewLine +
            "3. List all vehicles in all garages." + Environment.NewLine +
            "4. List all vehicles in a single garage." + Environment.NewLine +
            "5. List specific vehicles in all garages." + Environment.NewLine +
            "6. List specific vehicles in a single garage." + Environment.NewLine +
            "7. Add a new garage." + Environment.NewLine +
            "8. Access a specific garage to add or remove vehicles in it." + Environment.NewLine +
            "9. Find a specific vehicle by its license number." + Environment.NewLine;
        
        // File read
        public static string ReadSuccess => "The file \"SavedList\" was successfully found in the .exe directory.";
        public static string ReadNotFound => "The file \"SavedList\" was not found in the .exe directory.";
        public static string ReadNull => "The file \"SavedList\" was found in the .exe directory but was empty.";
        public static string ReadAddContents => "Would you like to add the contents from the file to the memory?";
        public static string ReadAddingContents => "Adding garages and vehicles from the file to the memory.";
        public static string ReadVehicleNull(int line) => $"A vehicle at line {line} could not be added.";
        public static string ReadFinishedAddingContent(int numberOfGarages, int numberOfVehicles) => $"{numberOfGarages} garages and {numberOfVehicles} vehicles were created.";

        // Create garage
        public static string CreateGarageSize => "Please enter how many parking spots you would like in the garage.";
        public static string PopulateGarage => "Would you like to populate the garage from the start?";
        public static string NumberOfVehicles => "How many vehicles would you like to add to the garage?";
        public static string NumberOfVehiclesTooMany => "You entered a number of vehicles that is larger than the size of the garage.";
        public static string AllPropertiesOfVehicles => "Would you like to set all the properties of the vehicle?" + Environment.NewLine +
                                                        "If you answer no you will only set the vehicle type, license number, and color.";
        public static string SetPropertyOfVehicle(string property, string type) => $"Please enter the {property} as {type}.";
        public static string GarageCreated(int spots) => $"You have created a garage with {spots} parking spots.";
        public static string GarageCreated(int spots, int vehicles) => $"You have created a garage with {spots} parking spots and {vehicles} vehicles in it.";

        // Populate garage
        public static string InputVehicleType => "Please enter the vehicle type.";
        public static string InputVehicleTypeNotFound(string vehicleType) => $"No vehicle type named {vehicleType} was found.";
        public static string InputVehicleLicenseNumber => "Please enter the vehicle's unique license number.";
        public static string InputVehicleLicenseNumberNotUnique => "Another vehicle already has that license number.";
        public static string InputVehicleColor => "Please enter the name of the vehicle's color.";
        public static string InputVehicleColorNotRecognized => $"The color was not recognized.";

        // Access garage
        public static string GarageAccessed(int index) => $"You have accessed garage {index}. Please choose an option." + Environment.NewLine +
                                                                 "0. Return to the main menu." + Environment.NewLine +
                                                                 "1. List all vehicles in the garage." + Environment.NewLine +
                                                                 "2. Park a vehicle in the garage." + Environment.NewLine +
                                                                 "3. Remove a vehicle from the garage.";
        public static string AccessingGarage(int index) => $"You are accessing garage {index}.";
        public static string RemoveVehicle => "Please enter the license number of the vehicle you want to remove from the garage.";
        public static string VehicleRemoved(string licenseNumber) => $"The vehicle with license number {licenseNumber} has been removed from the garage.";
        public static string VehicleRemoveFailed(string licenseNumber) => $"No vehicle with the license number {licenseNumber} was found in this garage.";

        // Create vehicle special
        public static string InputVehicleFuelType => "Please enter the fuel type of the vehicle.";
        public static string InputVehicleSizeProperty => $"Please enter the number of parking spots the vehicle requires.";
        public static string InputVehicleIntegerProperty(string property) => $"Please enter the {property} of the vehicle.";

        // Add vehicle
        public static string AddVehicleWhichGarage(int min, int max) => $"Please enter the garage ({min}-{max}) you would like to add the vehicle to.";
        public static string AddVehicleIsNull => "The vehicle was not a valid instance.";
        public static string AddVehicleGarageDoesNotExist(int index) => $"Garage {index} does not exist.";
        public static string AddVehicleSuccess(string licenseNumber, int index) => $"A vehicle with license number {licenseNumber} was added to garage {index}.";

        // UI Input
        public static string InputValidBool => "Please input yes to accept or no to reject.";
        public static string InputValidInt => "Please input an integer value.";
        public static string InputNotValid => "Your input was not recognized. Please try again.";

        // Find vehicle
        public static string FindVehicle => "Please enter the license number of the vehicle you wish to find.";
        public static string VehicleFound(int garageNumber) => $"The vehicle was found in garage {garageNumber}.";
        public static string VehicleNotFound => $"The vehicle was not found.";

        // List all vehicles
        public static string ListSpecificGarage(int min, int max) => $"Please input which garage ({min}-{max}) you would like to access.";
        public static string ListSpecificGarageDoesNotExist => "The garage you are trying to access does not exist.";
        public static string ListNoGarages => "There are no garages in memory.";
        public static string ListOnlyOneGarage => "There is only one garage in memory.";
        public static string ListEmptyGarage => "The garage is empty.";

        // List specific vehicles
        public static string SpecificVehicleByProperty() => "Which property would you like to select by?" + Environment.NewLine +
                                                            "0. Finish selection and move on to result." + Environment.NewLine +
                                                            "1. Vehicle type." + Environment.NewLine +
                                                            "2. Color." + Environment.NewLine +
                                                            "3. Size as number of parking spots it uses." + Environment.NewLine +
                                                            "4. Number of wheels." + Environment.NewLine +
                                                            "5. Fuel type." + Environment.NewLine;
        public static string SpecificVehicleEnterProperty(string property) => $"Which {property} would you like to select by?" + Environment.NewLine +
                                                                               "Enter 0 to return to the previous menu without making a selection." + Environment.NewLine;
        public static string SpecificVehicleEqualUpperOrLower(string property) => $"Would you like to include vehicles with {property} equal to your input," + Environment.NewLine +
                                                                                   "lower than your input, or higher than your input?" + Environment.NewLine +
                                                                                   "1. Equal." + Environment.NewLine +
                                                                                   "2. Equal or lower than." + Environment.NewLine +
                                                                                   "3. Equal or higher than." + Environment.NewLine;
        public static string SpecificVehicleAnotherProperty(string property) => $"Would you like to add another {property} to the selection?";

        // Error messages
        public static string ErrorNoValidInputIn100Tries => "No valid input given in 100 tries.";
    }
}
