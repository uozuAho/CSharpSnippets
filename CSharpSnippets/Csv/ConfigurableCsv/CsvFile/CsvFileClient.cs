using CSharpSnippets.Csv.ConfigurableCsv.CsvFile.Definition;
using System;
using System.Data;

namespace CSharpSnippets.Csv.ConfigurableCsv.CsvFile
{
    public class CsvFileClient
    {
        const string csvFilesDir = "Csv\\ConfigurableCsv\\CsvFiles";

        private CsvFileDefinition _csvDef;
        private CsvFile _csvFile;
        private DataTable _dataRows;

        private CsvFileClient(string definitionPath, string csvPath)
        {
            _csvDef = CsvFileDefinition.Load(definitionPath);
            _csvFile = new CsvFile(_csvDef, csvPath);
            _dataRows = CsvFileDefinition.CreateDataTable(_csvDef);
        }

        public static void Run()
        {
            var csv = new CsvFileClient(csvFilesDir + "\\PersonDefinition.xml", csvFilesDir + "\\Person.csv");
            csv.Process();
        }

        private void Process()
        {
            foreach (var row in _csvFile.ReadDataAndFooterRows())
            {
                if (row.AsStringArray()[0].StartsWith(_csvDef.Data.StartsWith))
                    ProcessDataRow(row);
                else if (row.AsStringArray()[0].StartsWith(_csvDef.Footer.StartsWith))
                    ProcessFooterRow(row);
            }
        }

        private void ProcessDataRow(ICsvRow row)
        {
            if (row.AsStringArray().Length != _csvDef.Data.Columns.Length)
            {
                OnRowError(row, $"expected {_csvDef.Data.Columns.Length} fields, found {row.AsStringArray().Length}");
                return;
            }
            var dataRow = _dataRows.NewRow();
            try
            {
                foreach (System.Data.DataColumn col in _dataRows.Columns)
                {
                    var value = row[col.ColumnName];
                    if (value != null)
                        dataRow[col.ColumnName] = value;
                    else
                        dataRow[col.ColumnName] = DBNull.Value;
                }
                _dataRows.Rows.Add(dataRow);
                Console.WriteLine("name: " + row["Name"]);
            }
            catch (ArgumentException e)
            {
                OnRowError(row, e.Message);
            }
            catch (NoNullAllowedException e)
            {
                OnRowError(row, e.Message);
            }
        }

        private void OnRowError(ICsvRow row, string msg)
        {
            Console.WriteLine($"Error reading row {row.RowNum}:");
            Console.WriteLine(string.Join(_csvDef.Options.Delimiter, row.AsStringArray()));
            Console.WriteLine(msg);
        }

        private void ProcessFooterRow(ICsvRow row)
        {
            var fRowCount = int.Parse(row[_csvDef.Footer.RowCountIndex]);
            var fChecksum = int.Parse(row[_csvDef.Footer.ChecksumIndex]);
            Console.WriteLine($"footer row count: {fRowCount}, checksum: {fChecksum}");
        }
    }
}
