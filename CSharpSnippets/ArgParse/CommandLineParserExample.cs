using CommandLine;

namespace CSharpSnippets.ArgParse
{
    /// <summary>
    /// Can't use CSharpSnippets.Args since CommandLineParser uses Attributes
    /// </summary>
    class CommandLineParserArgs
    {
        [Option]
        public int MyNum { get; set; }
        [Option]
        public string MyString { get; set; }
    }

    /// <summary>
    /// Meh, cbf getting started with this. Documentation is worse than FCLP, seems
    /// less configurable, can't see any killer features. Doesn't support positional
    /// args (neither does FCLP). Python's argparse is much better!!!
    /// </summary>
    class CommandLineParserExample
    {
    }
}
