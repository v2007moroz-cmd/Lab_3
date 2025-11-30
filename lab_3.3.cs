using System;
using System.Collections.Generic;

namespace SimpleTraffic
{
    public class Road
    {
        public string Name { get; }
        public double LengthKm { get; }
        public int Lanes { get; }

        public Road(string name, double lengthKm, int lanes)
        {
            Name = name;
            LengthKm = lengthKm;
            Lanes = lanes;
        }

        public override string ToString()
        {
            return $"{Name} (довжина {LengthKm} км, смуг {Lanes})";
        }
    }

    public interface IDriveable
    {
        void Move(double minutes);
        void Stop();
    }

    public class Vehicle : IDriveable
    {
        public string Id { get; }
        public string Type { get; }
        public double SpeedKmPerHour { get; private set; }
        public double PositionKm { get; private set; }
        public Road Road { get; }

        public Vehicle(string id, string type, double speed, Road road)
        {
            Id = id;
            Type = type;
            SpeedKmPerHour = speed;
            Road = road;
            PositionKm = 0;
        }

        public void Move(double minutes)
        {
            double distance = SpeedKmPerHour * (minutes / 60.0);
            PositionKm += distance;
            if (PositionKm > Road.LengthKm)
                PositionKm = Road.LengthKm;

            Console.WriteLine($"{Type} {Id} рухається по \"{Road.Name}\". Позиція: {PositionKm:F2} км.");
        }

        public void Stop()
        {
            SpeedKmPerHour = 0;
            Console.WriteLine($"{Type} {Id} зупинився на дорозі \"{Road.Name}\".");
        }
    }

    public class TrafficController
    {
        private readonly List<Vehicle> _vehicles = new();

        public void AddVehicle(Vehicle v) => _vehicles.Add(v);

        public void Simulate(double minutes)
        {
            Console.WriteLine($"\n=== Симуляція руху {minutes} хв ===");
            foreach (var v in _vehicles)
            {
                v.Move(minutes);
            }
        }
    }

    internal class Program
    {
        static void Main()
        {
            var mainRoad = new Road("Проспект Миру", 5, 3);
            var sideRoad = new Road("Вулиця Шевченка", 2, 1);

            var car = new Vehicle("A1", "Автомобіль", 60, mainRoad);
            var bus = new Vehicle("B1", "Автобус", 40, mainRoad);
            var truck = new Vehicle("C1", "Вантажівка", 50, sideRoad);

            var controller = new TrafficController();
            controller.AddVehicle(car);
            controller.AddVehicle(bus);
            controller.AddVehicle(truck);

            controller.Simulate(10); // 10 хв
            controller.Simulate(10);

            bus.Stop();

            Console.WriteLine("\n Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}