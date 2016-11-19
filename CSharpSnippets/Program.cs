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
using CSharpSnippets.Extensibility.Mef.CalculatorExample;
using CSharpSnippets.Database.MigSharp.Simple;
using CSharpSnippets.Database.MigSharp.MultiModule;

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

            // .NET framework
            //MefCalculatorExample.Run();

            // patterns
            //DoFactoryExample.Run();

            // database
            //SqlClientExamples.DataReaderExample();
            //SqlClientExamples.BulkCopyDataTableExamples();
            //DapperExamples.SimpleRead();
            //DataTableExamples.Run();
            //TransactionExamples.Run();
            //StoredProcs.Run();
            //MigSharpSimpleExample.Run(@"Data Source=localhost\sql2016;Initial Catalog=MigSharpExample;Integrated Security=SSPI;");
            MigSharpMultiModuleExample.Run(@"Data Source=localhost\sql2016;Initial Catalog=MigSharpExample;Integrated Security=SSPI;");

            //LoggingExample.Run();
            //FluentCommandLineParserExample.Run();
            //ProfilerTester.Run();
            //Csv.CsvHelperTest.Run();
            //CsvFileClient.Run();
            //XmlSerializerExample.Run();
            //XmlExtraAttributeExample.Run();

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
