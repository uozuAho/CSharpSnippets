﻿using CSharpSnippets.Database;
using CSharpSnippets.Logging;
using CSharpSnippets.ArgParse;
using CSharpSnippets.CSharpLanguage.GenericTypes;

namespace CSharpSnippets
{
    class Program
    {
        private static readonly ILogger log = LoggerFactory.GetLogger();

        static void Main(string[] args)
        {
            //LoggingExample();
            //SqlClientExamples.DataReaderExample();
            //DapperExamples.SimpleRead();
            //FluentCommandLineParserExample.Run();
            GenericTypeExample.Run();
        }

        static void LoggingExample()
        {
            log.Trace("hi from logger");
            log.Warn("warn");
            log.Error("error");
        }
    }
}
