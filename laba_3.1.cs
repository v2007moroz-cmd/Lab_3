using System;
using System.Collections.Generic;

namespace SimpleEcosystem
{
    public interface IReproducible
    {
        void Reproduce();
    }

    public interface IPredator
    {
        void Hunt(LivingOrganism prey);
    }

    public abstract class LivingOrganism
    {
        public string Name { get; }
        public double Energy { get; protected set; }
        public int Age { get; protected set; }
        public double Size { get; protected set; }

        protected LivingOrganism(string name, double energy, double size)
        {
            Name = name;
            Energy = energy;
            Size = size;
        }

        public virtual void LiveOneStep()
        {
            Age++;
            Energy -= 1;
            Console.WriteLine($"{Name} живе. Вік: {Age}, енергія: {Energy}");
        }
    }

    public class Animal : LivingOrganism, IReproducible, IPredator
    {
        public double Speed { get; }

        public Animal(string name, double energy, double size, double speed)
            : base(name, energy, size)
        {
            Speed = speed;
        }

        public void Reproduce()
        {
            Console.WriteLine($"{Name} розмножується (тварина).");
        }

        public void Hunt(LivingOrganism prey)
        {
            Console.WriteLine($"{Name} полює на {prey.Name}.");
        }
    }

    public class Plant : LivingOrganism, IReproducible
    {
        public bool IsFlowering { get; }

        public Plant(string name, double energy, double size, bool isFlowering)
            : base(name, energy, size)
        {
            IsFlowering = isFlowering;
        }

        public void Reproduce()
        {
            Console.WriteLine($"{Name} розмножується (рослина – насінням/спорами).");
        }

        public override void LiveOneStep()
        {
            base.LiveOneStep();
            Energy += 2; // фотосинтез
            Console.WriteLine($"{Name} отримує енергію від сонця. Енергія: {Energy}");
        }
    }

    public class Microorganism : LivingOrganism
    {
        public bool IsPathogenic { get; }

        public Microorganism(string name, double energy, double size, bool isPathogenic)
            : base(name, energy, size)
        {
            IsPathogenic = isPathogenic;
        }
    }

    public class Ecosystem
    {
        private readonly List<LivingOrganism> _organisms = new();

        public void Add(LivingOrganism organism) => _organisms.Add(organism);

        public void SimulateOneStep()
        {
            Console.WriteLine("\n=== КРОК ЕКОСИСТЕМИ ===");
            foreach (var org in _organisms)
            {
                org.LiveOneStep();
            }
        }
    }

    internal class Program
    {
        static void Main()
        {
            var ecosystem = new Ecosystem();

            var deer = new Animal("Олень", 10, 5, 3);
            var wolf = new Animal("Вовк", 12, 4, 4);
            var tree = new Plant("Дерево", 5, 10, true);
            var microbe = new Microorganism("Мікроб", 3, 0.1, true);

            ecosystem.Add(deer);
            ecosystem.Add(wolf);
            ecosystem.Add(tree);
            ecosystem.Add(microbe);

            ecosystem.SimulateOneStep();

            wolf.Hunt(deer);
            tree.Reproduce();

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}