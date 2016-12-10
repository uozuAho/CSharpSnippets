using ConsoleAppFramework.ConsoleCommand;
using System;

namespace ConsoleAppExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var executor = GetConsoleExecutor();
            RunWithArgString(executor, null);
            RunWithArgString(executor, "help-default");
            RunWithArgString(executor, "help-custom");
            RunWithArgString(executor, "help hello");
            RunWithArgString(executor, "hello");
            RunWithArgString(executor, "hello blah blah asdf 123");
            RunWithArgString(executor, "hello /s poop");
            Console.WriteLine("done");
        }

        private static void RunWithArgString(ConsoleCommandExecutor executor, string args)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("Running with args: " + args);
            var argArray = args == null
                ? new string[0]
                : args.Split();
            executor.Execute(argArray);
        }

        private static ConsoleCommandExecutor GetConsoleExecutor()
        {
            var executor = new ConsoleCommandExecutor();
            executor.RegisterEmptyCommand((args) => Console.WriteLine("No args!"));
            executor.RegisterCommand("help-default", new HelpCommand(executor));
            var customHelp = new CustomHelp(executor);
            executor.RegisterCommand("help-custom", customHelp);
            executor.RegisterCommand("help", customHelp);
            executor.RegisterCommand("hello", new HelloWorldCommand());
            return executor;
        }

        private class CustomHelp : HelpCommand
        {
            public CustomHelp(ConsoleCommandExecutor executor) : base(executor)
            {
            }
        }
    }
}
