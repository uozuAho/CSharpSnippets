using CSharpSnippets.Xml.XmlExtraAttributes;
using System.Xml.Serialization;

namespace CSharpSnippets.Xml
{
    [XmlRoot("MyOptions")]
    public class MyOptionsWithExtras
    {
        public SettingsWithExtras Settings { get; set; }
        public ProductWithExtras[] Products { get; set; }
    }

    public class SettingsWithExtras
    {
        [XmlElement(IsNullable = true)]
        [XmlRequired]
        public BgColourWithExtras BgColour { get; set; }
    }

    public class BgColourWithExtras
    {
        [XmlAttribute]
        public string value { get; set; }
    }

    public class ProductWithExtras
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
    }
}
