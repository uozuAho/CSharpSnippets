using System;

namespace CSharpSnippets.CSharpLanguage.ExtensionMethods
{
    public static class DemoExtension
    {
        public static int WordCount(this string str)
        {
            return str.Split(' ').Length;
        }
    }

    public class ExtensionMethodExample
    {
        public static void Run()
        {
            const string words = "1 2 3";
            Console.WriteLine($"num words in '{words}': {words.WordCount()}");
        }
    }
}
