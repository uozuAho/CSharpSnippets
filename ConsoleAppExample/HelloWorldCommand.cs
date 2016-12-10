using ConsoleAppTemplate.ConsoleCommand;
using System;

namespace ConsoleAppExample
{
    class HelloWorldCommand : BaseConsoleCommand
    {
        public override void Execute(string[] args)
        {
            Console.WriteLine("Hello world! args:");
            Console.WriteLine(string.Join(", ", args));
        }

        public override string GetHelp(string usagePrefix)
        {
            return "Usage: " + usagePrefix + " [options]";
        }

        public override string GetSummary()
        {
            return "Prints a message to console";
        }
    }
}
