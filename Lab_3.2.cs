using System;
using System.Collections.Generic;

namespace SimpleNetwork

{
    public interface IConnectable
    {
        void Connect(Computer other);
        void Disconnect(Computer other);
        void SendData(Computer to, string data);
    }

    public abstract class Computer : IConnectable
    {
        public string Ip { get; }
        public int Power { get; }
        public string OS { get; }

        protected List<Computer> connections = new();

        protected Computer(string ip, int power, string os)
        {
            Ip = ip;
            Power = power;
            OS = os;
        }

        public virtual void Connect(Computer other)
        {
            if (!connections.Contains(other))
            {
                connections.Add(other);
                Console.WriteLine($"{Info()} підключився до {other.Info()}");
            }
        }

        public virtual void Disconnect(Computer other)
        {
            if (connections.Remove(other))
            {
                Console.WriteLine($"{Info()} відключився від {other.Info()}");
            }
        }

        public virtual void SendData(Computer to, string data)
        {
            if (connections.Contains(to))
            {
                Console.WriteLine($"{Info()} передає \"{data}\" до {to.Info()}");
            }
            else
            {
                Console.WriteLine($"{Info()} не підключений до {to.Info()} – передача неможлива.");
            }
        }

        public abstract string Info();
    }

    public class Server : Computer
    {
        public Server(string ip, int power, string os) : base(ip, power, os) { }

        public override string Info() => $"Сервер {Ip}";
    }

    public class Workstation : Computer
    {
        public string User { get; }

        public Workstation(string ip, int power, string os, string user)
            : base(ip, power, os)
        {
            User = user;
        }

        public override string Info() => $"Робоча станція {User} ({Ip})";
    }

    public class Router : Computer
    {
        public Router(string ip, int power, string os) : base(ip, power, os) { }

        public override string Info() => $"Маршрутизатор ({Ip})";
    }

    public class Network
    {
        private readonly List<Computer> _devices = new();

        public void Add(Computer comp) => _devices.Add(comp);

        public void Connect(Computer a, Computer b)
        {
            a.Connect(b);
            b.Connect(a);
        }

        public void Send(Computer from, Computer to, string data)
        {
            from.SendData(to, data);
        }
    }

    internal class Program
    {
        static void Main()
        {
            var net = new Network();

            var server = new Server("192.168.0.1", 1000, "Linux");
            var ws = new Workstation("192.168.0.10", 400, "Windows", "Олена");
            var router = new Router("192.168.0.254", 200, "RouterOS");

            net.Add(server);
            net.Add(ws);
            net.Add(router);

            net.Connect(router, server);
            net.Connect(router, ws);

            net.Send(ws, server, "GET /index.html");
            net.Send(server, ws, "HTTP 200 OK");

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}