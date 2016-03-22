using System;
using System.Collections.Generic;

namespace CSharpSnippets.CSharpLanguage
{
    class Delegates
    {
        delegate void DoThing(string msg = "");

        private static void doStaticThing(string msg)
        {
            Console.WriteLine("Static thing. " + msg);
        }

        private void doInstanceThing(string msg)
        {
            Console.WriteLine("Instance thing. " + msg);
        }

        public static void Run()
        {
            Delegates delegates = new Delegates();
            var thingsToDo = new List<DoThing>();
            thingsToDo.Add(new DoThing(doStaticThing));
            thingsToDo.Add(new DoThing(delegates.doInstanceThing));
            thingsToDo.Add(delegate (string msg) { Console.WriteLine("Anonymous thing. " + msg); });
            thingsToDo.Add((string msg) => { Console.WriteLine("Lambda thing. " + msg); });
            Console.WriteLine("Separate delegates:");
            foreach (DoThing thing in thingsToDo)
            {
                thing();
            }

            // Delegates can be combined
            DoThing allThings = (string msg) => { };
            foreach (DoThing thing in thingsToDo)
            {
                allThings += thing;
            }
            Console.WriteLine("Combined delegates:");
            allThings();
        }
    }
}
