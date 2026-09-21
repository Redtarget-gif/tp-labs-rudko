using System.Runtime.CompilerServices;

internal class Program
{
    public class Car
    {
        private double _fuel;

        public string Model { get; }
        public double FuelConsumprion {  get; }
        public double Mileage { get; private set; }
        
        public Car (string model, double fuelConsumption)
        {
            if (fuelConsumption<=0)
                throw new ArgumentOutOfRangeException(nameof(fuelConsumption));
            Model = model;
            FuelConsumprion=fuelConsumption;
        }

        public void Refuel(double liters)
        {
            if (liters <= 0) throw new ArgumentOutOfRangeException(nameof(liters));
            _fuel += liters;
        }

        public double Drive(double distanceKM)
        {
            double maxDistance = _fuel / FuelConsumprion * 100;
            double actual = Math.Min(distanceKM,maxDistance);
            _fuel -= actual * FuelConsumprion / 100;
            Mileage += actual;
            return actual;
        }
    }
    private static void Main(string[] args)
    {
        var car = new Car("Lada Vesta", 7.1);
        car.Refuel(20);
        double d = car.Drive(500);
        Console.WriteLine($"{car.Model}: проехали {d:F0} км, пробег {car.Mileage:F0} км");

    }
}