using ConsoleAppFramework.Helpers;
using ConsoleAppFramework.ConsoleCommand;
using Fclp;
using System;
using System.Text;

namespace ConsoleAppExample
{
    class HelloWorldCommandOptions
    {
        public string MyString { get; set; }
        public bool MyBool { get; set; }
    }

    class HelloWorldCommand : BaseConsoleCommand
    {
        public override void Execute(string[] args)
        {
            Console.WriteLine("Hello world! args:");
            Console.WriteLine(string.Join(", ", args));
            var parser = GetOptionsParser();
            var parseResult = parser.Parse(args);
            if (parseResult.HasErrors)
            {
                Console.WriteLine("Error parsing options:");
                Console.WriteLine(parseResult.ErrorText);
            }
            else
            {
                var options = parser.Object;
                Console.WriteLine("Options parsed:");
                Console.WriteLine("string: " + options.MyString);
                Console.WriteLine("bool: " + options.MyBool);
            }
        }

        public override string GetHelp(string usagePrefix)
        {
            var sb = new StringBuilder();
            sb.Append("Usage: ").Append(usagePrefix).AppendLine(" [options]");
            sb.AppendLine();
            sb.AppendLine("Options:");
            sb.Append(FclpHelper.GetPrettyOptionsText(GetOptionsParser()));
            return sb.ToString();
        }

        public override string GetSummary()
        {
            return "Prints a message to console";
        }

        private FluentCommandLineParser<HelloWorldCommandOptions> GetOptionsParser()
        {
            var parser = new FluentCommandLineParser<HelloWorldCommandOptions>();

            parser.Setup(arg => arg.MyString)
                .As('s', "string")
                .WithDescription("Set my string");

            parser.Setup(arg => arg.MyBool)
                .As('b', "bool")
                .WithDescription("Set amy bool");

            return parser;
        }
    }
}
