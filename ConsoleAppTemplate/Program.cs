using ConsoleAppTemplate.ConsoleCommand;
using System;

namespace ConsoleAppTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            var executor = GetConsoleExecutor();
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("no args:");
            executor.Execute(new string[0]);
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("help:");
            executor.Execute(new[] { "help" });
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("help help:");
            executor.Execute(new[] { "help", "help" });
            Console.WriteLine("done");
        }

        private static ConsoleCommandExecutor GetConsoleExecutor()
        {
            var executor = new ConsoleCommandExecutor();
            executor.RegisterEmptyCommand((args) => Console.WriteLine("No args!"));
            var help = new HelpCommand(executor);
            executor.RegisterCommand("h", help);
            executor.RegisterCommand("help", help);
            return executor;
        }
    }
}
