using System;

namespace CSharpSnippets.CSharpLanguage.Attributes
{
    class AttributeExamples
    {
        public static void Run()
        {
            foreach (var a in typeof(MyClass).CustomAttributes)
            {
                if (a.AttributeType == typeof(MyClassDataAttribute))
                {
                    Console.WriteLine(a.ConstructorArguments[0].Value);
                    Console.WriteLine(a.NamedArguments[0].TypedValue.Value);
                }

                if (a.AttributeType == typeof(MyTagAttribute))
                {
                    Console.WriteLine("I was tagged");
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    class MyClassDataAttribute : Attribute
    {
        public readonly string Url;
        public string Topic { get; set; }

        public MyClassDataAttribute(string url)
        {
            Url = url;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    class MyTagAttribute : Attribute
    {
    }

    [MyClassData("asdf.com", Topic = "blah")]
    [MyTag]
    class MyClass
    {

    }
}
