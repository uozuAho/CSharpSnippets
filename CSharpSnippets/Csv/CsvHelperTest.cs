using CsvHelper;
using CsvHelper.TypeConversion;
using System;
using System.IO;

namespace CSharpSnippets.Csv
{
    class CsvRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Date: {2}", Id, Name, Date);
        }
    }

    class CsvHelperTest
    {
        private const string CSV_DATA_DIR = "Csv\\CsvFiles";

        public static void ReadAll()
        {
            var stream = new StreamReader(CSV_DATA_DIR + "\\HeaderAndFooter.csv");
            // skip header
            stream.ReadLine();
            var csv = new CsvReader(stream);
            // can't read all rows if a non-compliant footer exists
            // var rows = csv.GetRecords<CsvRow>();
            while (csv.Read())
            {
                try
                {
                    Console.WriteLine(csv.GetRecord<CsvRow>());
                }
                catch (CsvTypeConverterException)
                {
                    Console.WriteLine("Error converting row:");
                    Console.WriteLine(string.Join(csv.Configuration.Delimiter, csv.CurrentRecord));
                }
            }
        }

        private static void ReadAllWithError()
        {
            var stream = new StreamReader(CSV_DATA_DIR + "\\HeaderAndFooter.csv");
            // skip header
            stream.ReadLine();
            var csv = new CsvReader(stream);
            // can't read all rows if a non-compliant footer exists
            // var rows = csv.GetRecords<CsvRow>();
            while (csv.Read())
            {
                try
                {
                    var row = csv.GetRecord<CsvRow>();
                    ValidateRow(row);
                    Console.WriteLine(row);
                }
                catch (CsvTypeConverterException e)
                {
                    Console.WriteLine("Error converting row:");
                    Console.WriteLine(e.Data["CsvHelper"]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception while reading csv: " + e.Message);
                }
            }
        }

        private static void ValidateRow(CsvRow row)
        {
            if (row.Name.Length > 1)
                throw new Exception("Name too long");
        }

        public static void Run()
        {
            //ReadAll();
            ReadAllWithError();
        }
    }
}
