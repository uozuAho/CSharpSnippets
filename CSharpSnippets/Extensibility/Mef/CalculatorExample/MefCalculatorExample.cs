using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace CSharpSnippets.Extensibility.Mef.CalculatorExample
{
    class MefCalculatorExample : IDisposable
    {
        [Import(typeof(ICalculator))]
#pragma warning disable CS0649 // assigned by MEF
        public ICalculator calculator;
#pragma warning restore CS0649

        private CompositionContainer _container;

        private MefCalculatorExample()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));

            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }

        public static void Run()
        {
            var p = new MefCalculatorExample();
            string s;
            Console.WriteLine("Enter Commands. Press ctrl-c to exit.");
            while (true)
            {
                s = Console.ReadLine();
                Console.WriteLine(p.calculator.Calculate(s));
            }
        }

        public void Dispose()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}
