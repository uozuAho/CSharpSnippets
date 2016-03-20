using CSharpSnippets.Logging;

namespace CSharpSnippets
{
    class Program
    {
        private static readonly ILogger log = LoggerFactory.GetLogger();

        static void Main(string[] args)
        {
            log.Trace("hi from logger");
        }
    }
}
