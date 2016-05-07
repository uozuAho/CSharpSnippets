using CSharpSnippets.Database;
using CSharpSnippets.Logging;
using CSharpSnippets.ArgParse;
using CSharpSnippets.CSharpLanguage.Generics;
using System;
using CSharpSnippets.Profiling;
using CSharpSnippets.Patterns.AbstractFactory;
using CSharpSnippets.CSharpLanguage.Reflection;
using CSharpSnippets.Xml;
using CSharpSnippets.Csv.ConfigurableCsv.CsvFile;

namespace CSharpSnippets
{
    class Program
    {
        static void Main(string[] args)
        {
            //LoggingExample.Run();
            //SqlClientExamples.DataReaderExample();
            //SqlClientExamples.BulkCopyDataTableExamples();
            //DapperExamples.SimpleRead();
            //FluentCommandLineParserExample.Run();
            //GenericTypeExample.Run();
            //ProfilerTester.Run();
            //Csv.CsvHelperTest.Run();
            CsvFileClient.Run();
            //DataTableExamples.Run();
            // ReflectionExample.Run();
            //XmlSerializerExample.Run();

            // patterns
            //DoFactoryExample.Run();

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
