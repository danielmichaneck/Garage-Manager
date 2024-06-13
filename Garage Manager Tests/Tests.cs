using Garage_Manager;
using System.Drawing;

namespace Garage_Manager_Tests
{
    public class Tests
    {
        [Fact]
        public void Garage_Enumerator_Test_Add()
        {
            // Setup
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
            // Setup
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
            // Setup
            bool expectedFalse;

            IGarage<IVehicle> garage = new Garage<IVehicle>(1);

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("ABC 245", Color.Red);

            garage.Add(car1);

            expectedFalse = garage.Add(car2);

            // Assert
            Assert.False(expectedFalse);
        }

        [Fact]
        public void Garage_Enumerator_Test_Remove_Success()
        {
            // Setup
            bool expectedTrue;

            IGarage<IVehicle> garage = new Garage<IVehicle>(2);

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("ABC 245", Color.Red);

            garage.Add(car1);
            garage.Add(car2);

            expectedTrue = garage.Remove(car2.GetVehicleInformation()._licenseNumber);

            // Assert
            Assert.True(expectedTrue);
        }

        [Fact]
        public void Garage_Enumerator_Test_Remove_Fail()
        {
            // Setup
            bool expectedFalse;

            IGarage<IVehicle> garage = new Garage<IVehicle>(2);

            IVehicle car1 = new Car("ABC 123", Color.Green);
            IVehicle car2 = new Car("ABC 245", Color.Red);

            garage.Add(car1);

            expectedFalse = garage.Remove(car2.GetVehicleInformation()._licenseNumber);

            // Assert
            Assert.False(expectedFalse);
        }
    }
}