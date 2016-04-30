using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpSnippets.Utils.Extensions
{
    /// <summary>
    /// My dictionary utils
    /// </summary>
    public static class UozuDictionary
    {
        public static string ToPrettyString<K, V>(this Dictionary<K, V> dict, int indent = 4)
        {
            var nl = Environment.NewLine;
            var builder = new StringBuilder("{").Append(nl);
            foreach (var keyVal in dict)
            {
                builder
                    .Append("".PadLeft(indent))
                    .Append(keyVal.Key.ToString())
                    .Append(": ")
                    .Append(keyVal.Value == null ? "" : keyVal.Value.ToString())
                    .Append(nl);
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}
