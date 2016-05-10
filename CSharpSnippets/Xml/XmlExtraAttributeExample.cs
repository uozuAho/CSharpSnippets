using CSharpSnippets.Xml.XmlExtraAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace CSharpSnippets.Xml
{
    public class XmlExtraAttributeExample
    {
        public static void Run()
        {
            var xser = new XmlSerializer(typeof(MyOptionsWithExtras));
            var myOptions = (MyOptionsWithExtras)xser.Deserialize(new StreamReader("Xml\\XmlFiles\\MyOptions.xml"));
            ValidateExtraAttributes(myOptions);
        }

        // This is probably more trouble than it's worth. Try validation using XSD
        private static void ValidateExtraAttributes(object obj)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                var attributes = new Dictionary<Type, CustomAttributeData>();
                foreach (var attr in property.CustomAttributes)
                {
                    attributes[attr.AttributeType] = attr;
                }
                if (attributes.ContainsKey(typeof(XmlRequiredAttribute)))
                {
                    if (!attributes.ContainsKey(typeof(XmlElementAttribute)))
                        throw new Exception("XmlRequired attribute must be used with XmlElement or XmlAttribute attributes");
                }
                // This causes infinite recursion for array types
                if (!property.PropertyType.IsValueType)
                    ValidateExtraAttributes(property.GetValue(obj));
            }
        }
    }
}
