using System;

namespace ConsoleAppFramework.ConsoleCommand
{
    class AnonymousCommand : BaseConsoleCommand
    {
        private Action<string[]> _onExecute;
        private string _summary;

        public AnonymousCommand(Action<string[]> execute, string summary = null)
        {
            _onExecute = execute;
            _summary = summary;
        }

        public override void Execute(string[] args)
        {
            _onExecute(args);
        }

        public override string GetSummary()
        {
            return _summary;
        }

        public override string GetHelp(string usagePrefix)
        {
            return "Usage: " + usagePrefix;
        }
    }
}
