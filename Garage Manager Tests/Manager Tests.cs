using Garage_Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager_Tests
{
    public class Manager_Tests
    {
        Manager manager = new();

        [Fact]
        public void List_Vehicles_In_Garage_Test()
        {
            // Arrange
            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("DEF 456", Color.Brown);
            List<IVehicle> cars = [car1, car2];

            string expected = car1.GetVehicleInformation().ToString() + Environment.NewLine +
                              car2.GetVehicleInformation().ToString();

            IHandler handler = new Handler();

            // Act
            IGarage<IVehicle> newGarage = handler.CreateNewGarage(size: 10, cars);

            // Assert
            //ToDo: Fix
            //Assert.Equal(expected, manager.ListVehiclesInGarage(garage));
        }
    }
}
