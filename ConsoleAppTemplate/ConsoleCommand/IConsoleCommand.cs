namespace ConsoleAppTemplate.ConsoleCommand
{
    interface IConsoleCommand
    {
        string GetSummary();
        string GetHelp();
        void Execute(string[] args);
    }
}
