using Fclp;
using Fclp.Internals;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpSnippets.ArgParse
{
    class Args
    {
        public int MyNum { get; set; }
        public string MyString { get; set; }
    }

    public class FluentCommandLineParserExample
    {
        public static void Run()
        {
            Run(new string[] { "n", "5", "string", "meh" });
        }

        public static void Run(string[] args)
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
                .Callback(text => Console.WriteLine(text));

            p.Parse(args);

            p.HelpOption.ShowHelp(p.Options);
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
