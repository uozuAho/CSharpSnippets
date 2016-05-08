using CSharpSnippets.Csv.ConfigurableCsv.CsvFile.Definition;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;

namespace CSharpSnippets.Csv.ConfigurableCsv.CsvFile
{
    public interface ICsvRow
    {
        string[] AsStringArray();
        string this[string index] { get; }
        string this[int index] { get; }
    }

    class CsvRow : ICsvRow
    {
        public ICsvReaderRow Row { get; set; }

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

        /// <summary>
        /// Read all rows, ignoring header rows.
        /// </summary>
        public IEnumerable<ICsvRow> ReadRows()
        {
            using (var stream = new StreamReader(_path))
            {
                // skip header rows
                for (int i = 0; i < _definition.Header.NumRows; i++)
                    stream.ReadLine();
                using (var reader = new CsvReader(stream, CreateCsvConfiguration()))
                {
                    CsvRow row = new CsvRow();
                    while (reader.Read())
                    {
                        row.Row = reader;
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
