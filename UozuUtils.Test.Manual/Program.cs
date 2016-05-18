using System;
using System.Diagnostics;

namespace Uozu.Utils.Test.Manual
{
    class Program
    {
        static void Main(string[] args)
        {
            //SqlMapperSpeedTest.Run();

            var p1 = Process.Start(@"C:\uozu\CSharpSnippets\CSharpSnippets\bin\Debug\CSharpSnippets.exe");
            var p2 = Process.Start(@"C:\uozu\CSharpSnippets\CSharpSnippets\bin\Debug\CSharpSnippets.exe");

            p1.WaitForExit();
            p2.WaitForExit();

            Console.WriteLine($"p1 exited with: {p1.ExitCode}, p2 exited with: {p2.ExitCode}");

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
