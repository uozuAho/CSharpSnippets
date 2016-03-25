using System;

namespace CSharpSnippets.CSharpLanguage.GenericTypes
{
    class MyGenericType<T>
    {
        public T Thing { get; private set; }

        public MyGenericType(T thing)
        {
            Thing = thing;
        }
    }

    class GenericTypeExample
    {
        public static void Run()
        {
            var myType = new MyGenericType<int>(1);
            Console.WriteLine("My type's value is " + myType.Thing.ToString());
        }
    }
}
