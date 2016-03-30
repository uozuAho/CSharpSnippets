using System;

namespace CSharpSnippets.Logging
{
    class LoggingExample
    {
        private static readonly ILogger log = LoggerFactory.GetLogger();

        public static void Run()
        {
            log.Trace("hi from logger");
            log.Warn("warn");
            log.Error("error");
            try
            {
                Throw();
            }
            catch (Exception e)
            {
                log.Trace(e, "message with exception");
            }
        }

        static void Throw()
        {
            throw new Exception("blah");
        }
    }
}
