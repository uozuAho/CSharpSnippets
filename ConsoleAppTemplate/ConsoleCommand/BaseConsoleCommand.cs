﻿namespace ConsoleAppTemplate.ConsoleCommand
{
    abstract class BaseConsoleCommand : IConsoleCommand
    {
        public abstract string GetSummary();
        public abstract string GetHelp();
        public abstract void Execute(string[] args);
    }
}
