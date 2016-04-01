using System;

namespace CSharpSnippets.Patterns.AbstractFactory
{
    /// <summary>
    /// Taken from
    /// http://www.dofactory.com/net/abstract-factory-design-pattern
    /// </summary>
    internal class DoFactoryExample
    {
        /// <summary>
        /// Entry point into console application.
        /// </summary>
        public static void Run()
        {
            // Create and run the African animal world
            ContinentFactory africa = new AfricaFactory();
            AnimalWorld world = new AnimalWorld(africa);
            world.RunFoodChain();
            // Create and run the American animal world
            ContinentFactory america = new AmericaFactory();
            world = new AnimalWorld(america);
            world.RunFoodChain();
            // Wait for user input
            Console.ReadKey();
        }
    }
    /// <summary>
    /// The 'AbstractFactory' abstract class
    /// </summary>
    internal abstract class ContinentFactory
    {
        public abstract Herbivore CreateHerbivore();
        public abstract Carnivore CreateCarnivore();
    }
    /// <summary>
    /// The 'ConcreteFactory1' class
    /// </summary>
    internal class AfricaFactory : ContinentFactory
    {
        public override Herbivore CreateHerbivore()
        {
            return new Wildebeest();
        }
        public override Carnivore CreateCarnivore()
        {
            return new Lion();
        }
    }
    /// <summary>
    /// The 'ConcreteFactory2' class
    /// </summary>
    internal class AmericaFactory : ContinentFactory
    {
        public override Herbivore CreateHerbivore()
        {
            return new Bison();
        }
        public override Carnivore CreateCarnivore()
        {
            return new Wolf();
        }
    }
    /// <summary>
    /// The 'AbstractProductA' abstract class
    /// </summary>
    internal abstract class Herbivore
    {
    }
    /// <summary>
    /// The 'AbstractProductB' abstract class
    /// </summary>
    internal abstract class Carnivore
    {
        public abstract void Eat(Herbivore h);
    }
    /// <summary>
    /// The 'ProductA1' class
    /// </summary>
    internal class Wildebeest : Herbivore
    {
    }
    /// <summary>
    /// The 'ProductB1' class
    /// </summary>
    internal class Lion : Carnivore
    {
        public override void Eat(Herbivore h)
        {
            // Eat Wildebeest
            Console.WriteLine(this.GetType().Name +
              " eats " + h.GetType().Name);
        }
    }
    /// <summary>
    /// The 'ProductA2' class
    /// </summary>
    internal class Bison : Herbivore
    {
    }
    /// <summary>
    /// The 'ProductB2' class
    /// </summary>
    internal class Wolf : Carnivore
    {
        public override void Eat(Herbivore h)
        {
            // Eat Bison
            Console.WriteLine(this.GetType().Name +
              " eats " + h.GetType().Name);
        }
    }
    /// <summary>
    /// The 'Client' class
    /// </summary>
    internal class AnimalWorld
    {
        private Herbivore _herbivore;
        private Carnivore _carnivore;
        // Constructor
        public AnimalWorld(ContinentFactory factory)
        {
            _carnivore = factory.CreateCarnivore();
            _herbivore = factory.CreateHerbivore();
        }
        public void RunFoodChain()
        {
            _carnivore.Eat(_herbivore);
        }
    }
}