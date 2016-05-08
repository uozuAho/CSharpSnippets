using CSharpSnippets.Csv.ConfigurableCsv.CsvFile.Definition;
using System;
using System.Data;

namespace CSharpSnippets.Csv.ConfigurableCsv.CsvFile
{
    public class CsvFileClient
    {
        public static void Run()
        {
            UseReadPostHeaderRowsAsICsvRow();
        }

        private static void UseReadPostHeaderRowsAsICsvRow()
        {
            const string csvFilesDir = "Csv\\ConfigurableCsv\\CsvFiles";
            var csvdef = CsvFileDefinition.Load(csvFilesDir + "\\PersonDefinition.xml");
            var csvfile = new CsvFile(csvdef, csvFilesDir + "\\Person.csv");
            var datatable = CsvFileDefinition.CreateDataTable(csvdef);
            var rownum = 1 + csvdef.Header.NumRows;
            foreach (var row in csvfile.ReadRows())
            {
                var dr = datatable.NewRow();
                try
                {
                    foreach (DataColumn col in datatable.Columns)
                        dr[col.ColumnName] = row[col.ColumnName];
                    Console.WriteLine("name: " + row["Name"]);
                    rownum++;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Error reading row {rownum}:");
                    Console.WriteLine(string.Join(csvdef.Options.Delimiter, row.AsStringArray()));
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
