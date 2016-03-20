namespace CSharpSnippets.Logging
{
    class LoggerFactory
    {
        public static ILogger GetLogger(string name)
        {
            return new NLogLogger(name);
        }
    }
}
