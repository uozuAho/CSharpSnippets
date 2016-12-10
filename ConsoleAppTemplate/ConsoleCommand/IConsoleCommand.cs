namespace ConsoleAppTemplate.ConsoleCommand
{
    public interface IConsoleCommand
    {
        string GetSummary();
        string GetHelp(string usagePrefix);
        void Execute(string[] args);
    }
}
