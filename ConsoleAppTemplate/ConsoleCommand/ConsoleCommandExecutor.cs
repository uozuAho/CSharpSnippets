using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAppTemplate.ConsoleCommand
{
    public class ConsoleCommandExecutor
    {
        private IConsoleCommand _noArgsCommand;
        public Dictionary<string, IConsoleCommand> Commands = new Dictionary<string, IConsoleCommand>();

        /// <summary>
        /// Register command to execute with the given key
        /// </summary>
        public void RegisterCommand(string commandName, IConsoleCommand command)
        {
            if (Commands.ContainsKey(commandName))
                throw new InvalidOperationException(string.Format(
                    "A command with name '{0}' is already registered", commandName));
            Commands[commandName] = command;
        }

        public void RegisterCommand(string commandName, Action<string[]> action, string summary = null)
        {
            RegisterCommand(commandName, new AnonymousCommand(action, summary));
        }

        /// <summary>
        /// Register command to execute when no args are given
        /// </summary>
        public void RegisterEmptyCommand(IConsoleCommand command)
        {
            _noArgsCommand = command;
        }

        public void RegisterEmptyCommand(Action<string[]> action, string summary = null)
        {
            RegisterEmptyCommand(new AnonymousCommand(action, summary));
        }

        public IEnumerable<string> GetCommandNamesForCommand(IConsoleCommand command)
        {
            return Commands.Where(kv => kv.Value == command).Select(kv => kv.Key);
        }

        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                if (_noArgsCommand != null) _noArgsCommand.Execute(args);
                else throw new InvalidOperationException("No executor registered for zero arguments");
            }
            else
            {
                var commandKey = args[0];
                Execute(commandKey, args.Skip(1).ToArray());
            }
        }

        public void Execute(string commandName, string[] args)
        {
            if (!Commands.ContainsKey(commandName))
                throw new ArgumentException(string.Format("No command with name '{0}' registered", commandName));
            Commands[commandName].Execute(args);
        }
    }
}
