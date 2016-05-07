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
        public Column[] Columns { get; set; }
        public Footer Footer { get; set; }

        public static CsvFileDefinition Load(string path)
        {
            var xs = new XmlSerializer(typeof(CsvFileDefinition));
            return (CsvFileDefinition)xs.Deserialize(new StreamReader(path));
        }

        public static DataTable CreateDataTable(CsvFileDefinition definition)
        {
            var datatable = new DataTable();
            foreach (var c in definition.Columns)
            {
                var dtCol = new DataColumn
                {
                    ColumnName = c.Name,
                    DataType = GetDataColumnType(c.Type),
                    AllowDBNull = c.AllowNull
                };
                if (c.Type.Equals("string")) dtCol.MaxLength = c.MaxLength;
                datatable.Columns.Add(dtCol);
            }
            return datatable;
        }

        public static Type GetDataColumnType(string type)
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
        public bool FirstRowContainsColumnNames { get; set; }
        [XmlAttribute]
        public string Delimiter { get; set; }
    }

    public class Header
    {
        [XmlAttribute]
        public int NumRows { get; set; }
    }

    public class Column
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
    }
}
