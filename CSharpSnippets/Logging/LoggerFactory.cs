namespace CSharpSnippets.Logging
{
    class LoggerFactory
    {
        private static ILogger _defaultLogger = new NLogLogger("default");

        /// <summary>
        /// If you don't need a named logger, just use this one.
        /// </summary>
        public static ILogger GetLogger()
        {
            return _defaultLogger;
        }

        public static ILogger GetLogger(string name)
        {
            return new NLogLogger(name);
        }
    }
}
