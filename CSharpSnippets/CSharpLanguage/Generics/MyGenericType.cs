using System;
using System.Collections.Generic;

namespace CSharpSnippets.CSharpLanguage.Generics
{
    class MyGenericType<T>
    {
        public T Thing { get; private set; }

        public MyGenericType(T thing)
        {
            Thing = thing;
        }

        /// <summary>
        /// Generic method - make a list of type items of arbitrary type.
        /// Doesn't have to be part of a generic type.
        /// </summary>
        public List<K> MakeList<K>(K first, K second)
        {
            var l = new List<K>();
            l.Add(first);
            l.Add(second);
            return l;
        }
    }

    class GenericTypeExample
    {
        public static void Run()
        {
            var myType = new MyGenericType<int>(1);
            Console.WriteLine("My type's value is " + myType.Thing.ToString());
            // Types can be inferred:
            myType.MakeList("peach", "poo");
        }
    }
}
