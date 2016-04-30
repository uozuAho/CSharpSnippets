using CSharpSnippets.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CSharpSnippets.CSharpLanguage.Reflection
{
    public class ReflectionExample
    {
        public static void Run()
        {
            ReadPropertyCachingSpeedTest();
        }

        private static void PrintSillyModelStore()
        {
            foreach (var model in SillyModelStore.GetSillyModelsAsDicts())
            {
                Console.WriteLine(model.ToPrettyString());
            }
        }

        private static void PrintSillyModel()
        {
            Console.WriteLine("SillyModel:");
            foreach (var property in typeof(SillyModel).GetProperties())
            {
                Console.WriteLine(property.Name);
            }
        }

        private static void ReadPropertyCachingSpeedTest()
        {
            const int NUM_LOOPS = 1000000;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            DoThingsWithPropertyNames(typeof(SillyModel), NUM_LOOPS, (x) => GetPropertyNames(x));
            stopwatch.Stop();
            Console.WriteLine($"No cache.    {NUM_LOOPS} iterations took: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();
            stopwatch.Start();
            DoThingsWithPropertyNames(typeof(SillyModel), NUM_LOOPS, (x) => GetPropertyNamesCached(x));
            stopwatch.Stop();
            Console.WriteLine($"Using cache. {NUM_LOOPS} iterations took: {stopwatch.ElapsedMilliseconds} ms");
        }

        private static void DoThingsWithPropertyNames(Type t, int numIterations, Func<Type, List<string>> nameGetter)
        {
            for (int i = 0; i < numIterations; i++)
            {
                foreach (var name in nameGetter(t))
                {
                    // just some fluff to make sure this doesn't get optimised out
                    if (name.Equals("sheila"))
                    {
                        Console.WriteLine("shouldn't see this");
                    }
                }
            }
        }

        private static Dictionary<Type, List<string>> _typePropertyCache = new Dictionary<Type, List<string>>();

        private static List<string> GetPropertyNamesCached(Type t)
        {
            List<string> properties;
            if (!_typePropertyCache.TryGetValue(t, out properties))
            {
                properties = GetPropertyNames(t);
                _typePropertyCache[t] = properties;
            }
            return properties;
        }

        private static List<string> GetPropertyNames(Type t)
        {
            return t.GetProperties().Select(x => x.Name).ToList();
        }
    }
}
