using System;
using System.Collections.Generic;
using Bogus;

abstract class Vehicle
{
    public string RegistrationNumber { get; set; }

    public Vehicle(string registrationNumber)
    {
        RegistrationNumber = registrationNumber;
    }

    public abstract double CalParkingCharge(int hoursPark);
}

class Car : Vehicle
{
    public Car(string regNumber) : base(regNumber) { }

    public override double CalParkingCharge(int hoursPark)
    {
        const double minimumFees = 2.00;
        const double additionalFeesRate = 0.50;

        double totalCharge = minimumFees;

        if (hoursPark > 3)
        {
            totalCharge += (hoursPark - 3) * additionalFeesRate;
        }

        return Math.Min(totalCharge, 10.00);
    }
}

class Garage
{
    public List<Vehicle> Vehicles { get; set; }
    public double DailyTotal { get; set; }

    public Garage()
    {
        Vehicles = new List<Vehicle>();
        DailyTotal = 0;
    }

    public void ParkVehicle(Vehicle vehicle)
    {
        Vehicles.Add(vehicle);
    }

    public void DisplayParkCharges()
    {
        Console.WriteLine($"Parking Charges for Yesterday - {this.GetType().Name}:");

        foreach (var vehicle in Vehicles)
        {
            int hoursParked = GetRandomHoursParked();
            double parkingCharge = vehicle.CalParkingCharge(hoursParked);

            DailyTotal += parkingCharge;

            Console.WriteLine($"Registration Number: {vehicle.RegistrationNumber}, Hours Parked: {hoursParked}, Parking Charge: €{parkingCharge:F2}");
        }

        Console.WriteLine($"Total Receipts for {this.GetType().Name}: €{DailyTotal:F2}");
    }

    private int GetRandomHoursParked()
    {
        Random random = new Random();
        return random.Next(1, 24);
    }
}

class Program
{
    static void Main()
    {
        Garage garage1 = new Garage();
        Garage garage2 = new Garage();
        Garage garage3 = new Garage();

        GenerateAndParkVehicles(garage1, 10);
        GenerateAndParkVehicles(garage2, 6);
        GenerateAndParkVehicles(garage3, 8);

        garage1.DisplayParkCharges();
        garage2.DisplayParkCharges();
        garage3.DisplayParkCharges();

        double totalReceipts = garage1.DailyTotal + garage2.DailyTotal + garage3.DailyTotal;
        Console.WriteLine($"Total Receipts for All Garages: €{totalReceipts:F2}");
        Console.ReadLine();
    }

    static void GenerateAndParkVehicles(Garage garage, int count)
    {
        var faker = new Faker<Vehicle>()
            .CustomInstantiator(f => new Car(f.Random.Replace("??#####")));

        List<Vehicle> vehicles = faker.Generate(count);

        foreach (var vehicle in vehicles)
        {
            garage.ParkVehicle(vehicle);
        }
    }
}