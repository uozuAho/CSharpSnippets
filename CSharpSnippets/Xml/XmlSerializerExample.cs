using System;
using System.IO;
using System.Xml.Serialization;

namespace CSharpSnippets.Xml
{
    class XmlSerializerExample
    {
        public static void Run()
        {
            //XsdMyOptionsExample();
            //XsdCarsExample();
            ManualMyOptionsExample();
        }

        /// <summary>
        /// Doesn't work. HTF do you use xsd-generated classes? They're stupid.
        /// </summary>
        private static void XsdMyOptionsExample()
        {
            var xser = new XmlSerializer(typeof(MyOptions));
            var myOptions = (MyOptions)xser.Deserialize(new StreamReader("Xml\\XmlFiles\\MyOptions.xml"));

            //how to do this ? S ? S ?? arggh xsd is shit
            //foreach (var opt in (MyOptionsProducts)myOptions.Items[0])
            //    Console.WriteLine(opt);
        }

        /// <summary>
        /// This works. Pretty simple XML though.
        /// </summary>
        private static void XsdCarsExample()
        {
            var xser = new XmlSerializer(typeof(Cars));
            var cars = (Cars)xser.Deserialize(new StreamReader("Xml\\XmlFiles\\Cars.xml"));

            foreach (var car in cars.Items)
                Console.WriteLine(car.Model);
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
    }

    [XmlRoot("MyOptions")]
    public class MyHandCodedOptions
    {
        public Settings Settings { get; set; }
        public Product[] Products { get; set; }
    }

    public class Settings
    {
        public BgColour BgColour { get; set; }
    }

    public class BgColour
    {
        [XmlAttribute]
        public string value { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
    }
}
