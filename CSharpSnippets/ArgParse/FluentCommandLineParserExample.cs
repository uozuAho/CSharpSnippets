using Fclp;
using Fclp.Internals;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpSnippets.ArgParse
{
    public class FluentCommandLineParserExample
    {
        public static void Run()
        {
            Run("/n 5 /string meh");
            Console.WriteLine("---------------------------");
            Run("garbage args");
            Console.WriteLine("---------------------------");
            Run("-h");
        }

        private static void Run(string command)
        {
            Console.WriteLine($"parsing '{command}'");
            var argArray = command.Split();
            Run(argArray);
        }

        private static void Run(string[] args)
        {
            var p = new FluentCommandLineParser<Args>();

            p.Setup(arg => arg.MyNum)
                .As('n', "number")
                .SetDefault(3)
                .WithDescription("Set a number");

            p.Setup(arg => arg.MyString)
                .As('s', "string")
                .SetDefault("empty")
                .WithDescription("Set a string");

            p.SetupHelp("h", "help")
                .WithCustomFormatter(new OptionsFormatter())
                .Callback(text => PrintHelpText(text));

            var result = p.Parse(args);
            if (result.HasErrors)
            {
                Console.WriteLine(result.ErrorText);
            }
            else if (!result.HelpCalled)
            {
                var parsedArgs = p.Object;
                Console.WriteLine("Read args.");
                Console.WriteLine($"number: {parsedArgs.MyNum}");
                Console.WriteLine($"string: {parsedArgs.MyString}");
            }
        }

        private static void PrintHelpText(string optionsText)
        {
            Console.WriteLine("Help text. Options:");
            Console.WriteLine(optionsText);
        }
    }

    class OptionsFormatter : ICommandLineOptionFormatter
    {
        public string Format(IEnumerable<ICommandLineOption> options)
        {
            var nl = Environment.NewLine;
            var text = new StringBuilder();

            foreach (var o in options)
            {
                var optionText = "";
                if (o.HasShortName)
                    optionText += "  -" + o.ShortName + " VALUE";
                if (o.HasLongName)
                    optionText += ", --" + o.LongName + " VALUE";

                text.Append(optionText.PadRight(30));
                text.Append(o.Description);
                text.Append(nl);
            }
            return text.ToString();
        }
    }
}
