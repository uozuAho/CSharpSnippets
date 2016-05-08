using System;
using System.Data;
using System.IO;
using System.Xml.Serialization;

namespace CSharpSnippets.Csv.ConfigurableCsv.CsvFile.Definition
{
    [XmlRoot("CsvFileDefinition")]
    public class CsvFileDefinition
    {
        public Options Options { get; set; }
        public Header Header { get; set; }
        public Data Data { get; set; }
        public Footer Footer { get; set; }

        public static CsvFileDefinition Load(string path)
        {
            var xs = new XmlSerializer(typeof(CsvFileDefinition));
            return (CsvFileDefinition)xs.Deserialize(new StreamReader(path));
        }

        public static DataTable CreateDataTable(CsvFileDefinition definition)
        {
            var datatable = new DataTable();
            foreach (var c in definition.Data.Columns)
            {
                var dtCol = new System.Data.DataColumn
                {
                    ColumnName = c.Name,
                    DataType = StringToType(c.Type),
                    AllowDBNull = c.AllowNull
                };
                if (c.Type.Equals("string")) dtCol.MaxLength = c.MaxLength;
                datatable.Columns.Add(dtCol);
            }
            return datatable;
        }

        private static Type StringToType(string type)
        {
            switch (type)
            {
                case "string":
                    return typeof(string);
                case "int":
                    return typeof(int);
                case "DateTime":
                    return typeof(DateTime);
                case "long":
                    return typeof(long);
                default:
                    throw new ArgumentException("Unknown type: " + type);
            }
        }
    }

    public class Options
    {
        [XmlAttribute]
        public bool FirstDataRowIsColumnHeadings { get; set; }
        [XmlAttribute]
        public string Delimiter { get; set; }
    }

    public class Header
    {
        [XmlAttribute]
        public int NumRows { get; set; }
        [XmlAttribute]
        public string StartsWith { get; set; }
        public int NameIndex { get; set; }
    }

    public class Data
    {
        [XmlAttribute]
        public string StartsWith { get; set; }
        public DataColumn[] Columns { get; set; }
    }

    public class DataColumn
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Type { get; set; }
        [XmlAttribute]
        public bool AllowNull { get; set; }
        [XmlAttribute]
        public int MaxLength { get; set; }
    }

    public class Footer
    {
        [XmlAttribute]
        public int NumRows { get; set; }
        [XmlAttribute]
        public string StartsWith { get; set; }
        public int RowCountIndex { get; set; }
        public int ChecksumIndex { get; set; }
        public string ChecksumName { get; set; }
    }
}
