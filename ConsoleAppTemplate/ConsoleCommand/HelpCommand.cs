using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleAppTemplate.ConsoleCommand
{
    class HelpCommand : IConsoleCommand
    {
        private ConsoleCommandExecutor _executor;

        public HelpCommand(ConsoleCommandExecutor executor)
        {
            _executor = executor;
        }

        public string GetSummary() { return "print this help"; }

        public string GetHelp() { return "no help for help :)"; }

        public void Execute(string[] args)
        {
            if (args.Length == 0)
                PrintNoArgHelp();
            else if (args.Length == 1)
                PrintCommandHelp(args[0]);
            else
                throw new ArgumentException("Invalid arguments");
        }

        protected void PrintNoArgHelp()
        {
            Console.WriteLine();
            Console.WriteLine(GetHeaderText());
            Console.WriteLine();
            Console.WriteLine("Usage: " + GetUsageText());
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            Console.WriteLine(GetAvailableCommandsText());
        }

        protected void PrintCommandHelp(string command)
        {
            if (!_executor.Commands.ContainsKey(command))
                Console.WriteLine("Unknown command: " + command);
            Console.WriteLine(_executor.Commands[command].GetHelp());
        }

        protected string GetHeaderText()
        {
            var name = Assembly.GetExecutingAssembly().GetName().Name;
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return name + ", " + version;
        }

        protected string GetUsageText()
        {
            var exe = Assembly.GetExecutingAssembly().GetName().Name;
            return exe + " command [options]";
        }

        protected string GetAvailableCommandsText()
        {
            var sb = new StringBuilder();
            var maxKeyWidth = _executor.Commands.Keys.Max(k => k.Length);
            foreach (var c in _executor.Commands)
            {
                sb.Append("  ").Append(c.Key.PadRight(maxKeyWidth + 1)).Append(": ").AppendLine(c.Value.GetSummary());
            }
            return sb.ToString();
        }
    }
}
