using CSharpSnippets.Logging;
using System;

namespace CSharpSnippets
{
    class Program
    {
        private static readonly ILogger log = LoggerFactory.GetLogger("log");

        static void Main(string[] args)
        {
            log.Trace("hi from logger");
        }
    }
}
