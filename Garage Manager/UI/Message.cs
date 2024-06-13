using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager
{
    public struct Message
    {
        public static string Start => "Please select an option from the menu." +
            "1. Read data from a file." +
            "2. Create a new garage." +
            "3. Exit the application.";

        public static string PopulateGarageQuestion => "Would you like to populate the garage from the start?";
    }
}
