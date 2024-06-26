using Garage_Manager;
using System.Drawing;

namespace Garage_Manager_Tests
{
    public class Garage_Tests
    {
        [Fact]
        public void Garage_Enumerator_Test_Add_Success()
        {
            // Arrange
            string expected;
            string output = "";

            IGarage<IVehicle> garage = new Garage<IVehicle>(10);

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("ABC 245", Color.Red);
            IVehicle car3 = new Car("ABC 517", Color.RebeccaPurple);
            IVehicle car4 = new Car("ABC 117", Color.MintCream);

            garage.Add(car1);
            garage.Add(car2);
            garage.Add(car3);
            garage.Add(car4);

            expected = car1.GetVehicleInformation().ToString();
            expected += car2.GetVehicleInformation().ToString();
            expected += car3.GetVehicleInformation().ToString();
            expected += car4.GetVehicleInformation().ToString();

            // Act
            foreach (IVehicle vehicle in garage)
            {
                output += vehicle.GetVehicleInformation().ToString();
            }

            // Assert
            Assert.Equal(output, expected);
        }

        [Fact]
        public void Garage_Enumerator_Test_OOB_Success()
        {
            // Arrange
            bool oob = false;
            string output = "";

            IGarage<IVehicle> garage = new Garage<IVehicle>(1);

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("ABC 245", Color.Red);
            IVehicle car3 = new Car("ABC 517", Color.RebeccaPurple);
            IVehicle car4 = new Car("ABC 117", Color.MintCream);

            garage.Add(car1);
            garage.Add(car2);
            garage.Add(car3);
            garage.Add(car4);

            // Act
            try
            {
                foreach (IVehicle vehicle in garage)
                {
                    output += vehicle.GetVehicleInformation();
                }
            }
            catch
            {
                oob = true;
            }

            // Assert
            Assert.False(oob);
        }

        [Fact]
        public void Garage_Enumerator_Test_Add_Fail()
        {
            // Arrange
            bool expectedFalse;

            IGarage<IVehicle> garage = new Garage<IVehicle>(1);

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("ABC 245", Color.Red);

            // Act
            garage.Add(car1);
            expectedFalse = garage.Add(car2);

            // Assert
            Assert.False(expectedFalse);
        }

        [Fact]
        public void Garage_Enumerator_Test_Remove_Success()
        {
            // Arrange
            bool expectedTrue;

            IGarage<IVehicle> garage = new Garage<IVehicle>(2);

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("ABC 245", Color.Red);

            // Act
            garage.Add(car1);
            garage.Add(car2);

            expectedTrue = garage.Remove(car2.GetVehicleInformation().LicenseNumber);

            // Assert
            Assert.True(expectedTrue);
        }

        [Fact]
        public void Garage_Enumerator_Test_Remove_Fail()
        {
            // Arrange
            bool expectedFalse;

            IGarage<IVehicle> garage = new Garage<IVehicle>(2);

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("ABC 245", Color.Red);

            // Act
            garage.Add(car1);

            expectedFalse = garage.Remove(car2.GetVehicleInformation().LicenseNumber);

            // Assert
            Assert.False(expectedFalse);
        }

        [Fact]
        public void Garage_Enumerator_Test_Initialize_With_Vehicles_Success()
        {
            // Arrange
            bool expectedTrue;

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("ABC 245", Color.Red);
            IVehicle[] cars = [car1, car2];

            IGarage<IVehicle> garage = new Garage<IVehicle>(2, cars);

            // Act
            expectedTrue = garage.Remove(car2.GetVehicleInformation().LicenseNumber);

            // Assert
            Assert.True(expectedTrue);
        }

        [Fact]
        public void Garage_Enumerator_Test_Count_Success()
        {
            // Arrange
            bool expectedTrue;

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("ABC 245", Color.Red);

            // Act
            IGarage<IVehicle> garage = new Garage<IVehicle>(4);

            garage.Add(car1);
            garage.Add(car2);

            expectedTrue = (garage.Count() == 2);

            // Assert
            Assert.True(expectedTrue);
        }

        [Fact]
        public void Garage_Enumerator_Test_Enumerate_Success()
        {
            // Arrange
            int result = 0;
            bool expectedTrue;

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("ABC 245", Color.Red);
            IVehicle[] cars = [car1, car2];
            IGarage<IVehicle> garage = new Garage<IVehicle>(2, cars);

            // Act
            foreach (IVehicle car in garage)
                result++;

            expectedTrue = (result == 2);

            // Assert
            Assert.True(expectedTrue);
        }
    }
}