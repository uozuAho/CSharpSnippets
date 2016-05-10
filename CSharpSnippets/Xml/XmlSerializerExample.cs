using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CSharpSnippets.Xml
{
    class XmlSerializerExample
    {
        public static void Run()
        {
            //ManualMyOptionsExample();
            Console.WriteLine("Validating xml...");
            ValidateXml("Xml\\MyOptions.xsd", "Xml\\XmlFiles\\MyOptionsBad.xml");
        }

        /// <summary>
        /// OMFG finally a solution that works and is easy to use. xsd.exe sounds good in theory, however I
        /// can't find one example online that does what this example does. Everyone stops at
        /// 'use xsd.exe to generate your classes', but no one shows how to use the resulting classes.
        /// </summary>
        public static void ManualMyOptionsExample()
        {
            var xser = new XmlSerializer(typeof(MyHandCodedOptions));
            var myOptions = (MyHandCodedOptions)xser.Deserialize(new StreamReader("Xml\\XmlFiles\\MyOptions.xml"));

            Console.WriteLine(myOptions.Settings.BgColour.value);

            foreach (var product in myOptions.Products)
                Console.WriteLine($"{product.Name}: {product.Price}");
        }

        public static void ValidateXml(string xsdPath, string xmlPath)
        {
            var schemas = new XmlSchemaSet();
            schemas.Add(null, xsdPath);
            var xml = XDocument.Load(xmlPath);
            var errors = new List<string>();
            xml.Validate(schemas, (obj, args) =>
            {
                errors.Add(args.Message);
            });
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
        }
    }
}
