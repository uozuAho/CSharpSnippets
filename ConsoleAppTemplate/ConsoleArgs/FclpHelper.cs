using Fclp;
using Fclp.Internals;
using Fclp.Internals.Extensions;
using System;
using System.Linq;
using System.Text;

namespace ConsoleAppTemplate.Args
{
    class FclpHelper
    {
        /// <summary>
        /// Returns a nicely-formatted string of options information for the given parser
        /// </summary>
        public static string GetPrettyOptionsText<T>(IFluentCommandLineParser<T> parser) where T : new()
        {
            const string optionLeftPad = "    ";
            var maxWidth = Console.WindowWidth;
            var b = new StringBuilder();
            var descriptionLeftMargin = parser.Options.Max(o => optionLeftPad.Length + GetParametersString(o).Length + 2);
            foreach (var o in parser.Options)
            {
                b.Append((optionLeftPad + GetParametersString(o)).PadRight(descriptionLeftMargin));
                b.Append(Wrap(o.Description, descriptionLeftMargin, maxWidth)).AppendLine();
            }
            return b.ToString();
        }

        private static string GetParametersString(ICommandLineOption option)
        {
            var b = new StringBuilder();
            if (!option.ShortName.IsNullOrEmpty())
                b.Append("/").Append(option.ShortName);
            if (!option.LongName.IsNullOrEmpty())
            {
                b.Append(option.ShortName.IsNullOrEmpty() ? "/" : ", /");
                b.Append(option.LongName);
            }
            return b.ToString();
        }

        private static string Wrap(string text, int leftMargin, int maxWidth)
        {
            if (text.Length + leftMargin < maxWidth)
                return text;
            var words = text.SplitOnWhitespace();
            var currentColumnNumber = text.Length + leftMargin;
            var newText = new StringBuilder();
            foreach (var word in words)
            {
                if (currentColumnNumber + word.Length + 1 >= maxWidth)
                {
                    newText.AppendLine().Append("".PadLeft(leftMargin));
                    currentColumnNumber = leftMargin;
                }
                newText.Append(word).Append(' ');
                currentColumnNumber += word.Length + 1;
            }
            return newText.ToString();
        }
    }
}
