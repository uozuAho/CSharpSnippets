namespace ConsoleAppFramework.ConsoleCommand
{
    public abstract class BaseConsoleCommand : IConsoleCommand
    {
        public abstract string GetSummary();
        public abstract string GetHelp(string usagePrefix);
        public abstract void Execute(string[] args);
    }
}
