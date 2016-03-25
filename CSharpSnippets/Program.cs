using CSharpSnippets.Database;
using CSharpSnippets.Logging;

namespace CSharpSnippets
{
    class Program
    {
        private static readonly ILogger log = LoggerFactory.GetLogger();

        static void Main(string[] args)
        {
            LoggingExample();
            //SqlClientExamples.DataReaderExample();
            //DapperExamples.SimpleRead();
        }

        static void LoggingExample()
        {
            log.Trace("hi from logger");
            log.Warn("warn");
            log.Error("error");
        }
    }
}
