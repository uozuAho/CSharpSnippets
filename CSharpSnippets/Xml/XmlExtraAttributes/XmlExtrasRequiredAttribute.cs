using System;

namespace CSharpSnippets.Xml.XmlExtraAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    class XmlRequiredAttribute : Attribute
    {
    }
}
