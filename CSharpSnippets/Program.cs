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
using CSharpSnippets.CSharpLanguage.Attributes;

namespace CSharpSnippets
{
    class Program
    {
        static void Main(string[] args)
        {
            // language
            //AttributeExamples.Run();
            //GenericTypeExample.Run();
            // ReflectionExample.Run();

            //LoggingExample.Run();
            //SqlClientExamples.DataReaderExample();
            //SqlClientExamples.BulkCopyDataTableExamples();
            //DapperExamples.SimpleRead();
            //FluentCommandLineParserExample.Run();
            //ProfilerTester.Run();
            //Csv.CsvHelperTest.Run();
            //CsvFileClient.Run();
            //DataTableExamples.Run();
            XmlSerializerExample.Run();
            //XmlExtraAttributeExample.Run();

            // patterns
            //DoFactoryExample.Run();

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
