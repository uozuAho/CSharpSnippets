using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleAppFramework.ConsoleCommand
{
    public class HelpCommand : IConsoleCommand
    {
        private ConsoleCommandExecutor _executor;

        public HelpCommand(ConsoleCommandExecutor executor)
        {
            _executor = executor;
        }

        public string GetSummary() { return "print this help"; }

        public string GetHelp(string prefix) { return "no help for help :)"; }

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
            Console.WriteLine(_executor.Commands[command].GetHelp(GetUsagePrefix(command)));
        }

        protected string GetHeaderText()
        {
            var name = Assembly.GetEntryAssembly().GetName().Name;
            var version = Assembly.GetEntryAssembly().GetName().Version;
            return name + ", " + version;
        }

        protected string GetUsageText()
        {
            var exe = Assembly.GetEntryAssembly().GetName().Name;
            return exe + " command [options]";
        }

        protected string GetUsagePrefix(string command)
        {
            var exe = Assembly.GetEntryAssembly().GetName().Name;
            return exe + " " + command;
        }

        protected string GetAvailableCommandsText()
        {
            var sb = new StringBuilder();
            var commandToNames = GetCommandToCommandNamesMap();
            var maxCommandNamesWidth = commandToNames.Max(kv => GetCommandNamesText(kv.Value).Length);
            foreach (var kv in commandToNames)
            {
                var command = kv.Key;
                var commandNames = GetCommandNamesText(kv.Value);
                sb.Append("  ")
                    .Append(commandNames.PadRight(maxCommandNamesWidth + 1))
                    .Append(": ")
                    .AppendLine(command.GetSummary());
            }
            return sb.ToString();
        }

        private Dictionary<IConsoleCommand, List<string>> GetCommandToCommandNamesMap()
        {
            var commandToNames = new Dictionary<IConsoleCommand, List<string>>();
            foreach (var kv in _executor.Commands)
            {
                var command = kv.Value;
                var commandName = kv.Key;
                if (commandToNames.ContainsKey(command))
                    commandToNames[command].Add(commandName);
                else
                    commandToNames[command] = new List<string> { commandName };
            }
            return commandToNames;
        }

        private string GetCommandNamesText(List<string> commandNames)
        {
            return string.Join(",", commandNames);
        }
    }
}
