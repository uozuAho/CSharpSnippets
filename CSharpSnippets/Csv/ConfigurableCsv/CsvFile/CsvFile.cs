using CSharpSnippets.Csv.ConfigurableCsv.CsvFile.Definition;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;

namespace CSharpSnippets.Csv.ConfigurableCsv.CsvFile
{
    public interface ICsvRow
    {
        int RowNum { get; }
        string[] AsStringArray();
        string this[string index] { get; }
        string this[int index] { get; }
    }

    class CsvRow : ICsvRow
    {
        public ICsvReaderRow Row { get; set; }
        public int RowNum { get; set; }

        public CsvRow() {}

        public string[] AsStringArray()
        {
            return Row.CurrentRecord;
        }

        public string this[string index]
        {
            get { return Row[index]; }
        }

        public string this[int index]
        {
            get { return Row[index]; }
        }
    }

    public class CsvFile
    {
        private CsvFileDefinition _definition;
        private string _path;

        public CsvFile(CsvFileDefinition definition, string path)
        {
            _definition = definition;
            _path = path;
        }

        public IEnumerable<ICsvRow> ReadDataAndFooterRows()
        {
            int rownum = 0;
            using (var stream = new StreamReader(_path))
            {
                // skip header rows
                for (int i = 0; i < _definition.Header.NumRows; i++)
                {
                    rownum++;
                    stream.ReadLine();
                }
                using (var reader = new CsvReader(stream, CreateCsvConfiguration()))
                {
                    if (_definition.Options.FirstDataRowIsColumnHeadings)
                        rownum++;
                    CsvRow row = new CsvRow();
                    while (reader.Read())
                    {
                        rownum++;
                        row.Row = reader;
                        row.RowNum = rownum;
                        yield return row;
                    }
                }
            }
        }

        private CsvConfiguration CreateCsvConfiguration()
        {
            return new CsvConfiguration
            {
                Delimiter = _definition.Options.Delimiter,
                HasHeaderRecord = _definition.Options.FirstDataRowIsColumnHeadings
            };
        }
    }
}
