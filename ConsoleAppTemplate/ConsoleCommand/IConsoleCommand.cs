namespace ConsoleAppTemplate.ConsoleCommand
{
    public interface IConsoleCommand
    {
        string GetSummary();
        string GetHelp();
        void Execute(string[] args);
    }
}
