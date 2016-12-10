using ConsoleAppTemplate.ConsoleCommand;
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

        // TODO: move to own file, add options
        private class HelloWorldCommand : BaseConsoleCommand
        {
            public override void Execute(string[] args)
            {
                Console.WriteLine("Hello world! args:");
                Console.WriteLine(string.Join(", ", args));
            }

            public override string GetHelp()
            {
                return "hmm";
            }

            public override string GetSummary()
            {
                return "Prints a message to console";
            }
        }
    }
}
