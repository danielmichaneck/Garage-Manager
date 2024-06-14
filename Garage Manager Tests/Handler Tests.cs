using Garage_Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Manager_Tests
{
    public class Handler_Tests
    {
        [Fact]
        public void List_Vehicles_In_Garage_Test()
        {
            // Arrange
            IHandler handler = new Handler();

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("DEF 456", Color.Brown);
            List<IVehicle> cars = [car1, car2];

            string expected = car1.GetVehicleInformation().ToString() + Environment.NewLine +
                              car2.GetVehicleInformation().ToString();

            // Act
            IGarage<IVehicle> newGarage = handler.CreateNewGarage(size: 10, cars);

            // Assert
            Assert.Equal(expected, handler.ListAllVehiclesInGarage(0).ToString());
        }

        [Fact]
        public void List_Vehicles_In_All_Garages_Test()
        {
            // Arrange
            IHandler handler = new Handler();

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("DEF 456", Color.Brown);
            IVehicle car3 = new Car("CAR 001", Color.Blue);
            IVehicle car4 = new Car("BIL 002", Color.Yellow);
            List<IVehicle> cars1 = [car1, car2];
            List<IVehicle> cars2 = [car3, car4];

            string expected = car1.GetVehicleInformation().ToString() + Environment.NewLine +
                              car2.GetVehicleInformation().ToString() + Environment.NewLine +
                              car3.GetVehicleInformation().ToString() + Environment.NewLine +
                              car4.GetVehicleInformation().ToString();

            StringBuilder result = new("");

            // Act
            IGarage<IVehicle> newGarage1 = handler.CreateNewGarage(size: 10, cars1);
            IGarage<IVehicle> newGarage2 = handler.CreateNewGarage(size: 10, cars2);

            foreach(IVehicle car in newGarage1)
            {
                result.Append(car.GetVehicleInformation().ToString() + Environment.NewLine);
            }

            foreach (IVehicle car in newGarage2)
            {
                result.Append(car.GetVehicleInformation().ToString() + Environment.NewLine);
            }

            // Assert
            Assert.Equal(expected, result.ToString().Trim());
        }
    }
}
