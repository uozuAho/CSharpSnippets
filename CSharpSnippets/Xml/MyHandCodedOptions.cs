using System.Xml.Serialization;

namespace CSharpSnippets.Xml
{
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
