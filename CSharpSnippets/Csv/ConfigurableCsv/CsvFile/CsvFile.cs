using CSharpSnippets.Csv.ConfigurableCsv.CsvFile.Definition;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CSharpSnippets.Csv.ConfigurableCsv.CsvFile
{
    public interface ICsvRow
    {
        string this[string index] { get; }
    }

    class CsvRow : ICsvRow
    {
        public ICsvReaderRow Row { get; set; }

        public CsvRow() {}

        public string this[string index]
        {
            get { return Row[index]; }
        }
    }

    public class CsvFile
    {
        private CsvFileDefinition _definition;
        private string _path;
        private Dictionary<string, int> _columnOrdinalMap;

        public CsvFile(CsvFileDefinition definition, string path)
        {
            _definition = definition;
            _path = path;
        }

        public IEnumerable<ICsvRow> ReadPostHeaderRowsAsICsvRow()
        {
            using (var stream = new StreamReader(_path))
            {
                // skip header. TODO: header parsing
                for (var i = 0; i < _definition.Header.NumRows; i++)
                    stream.ReadLine();

                using (var reader = new CsvReader(stream, CreateCsvConfiguration(_definition)))
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

        public IEnumerable<DataRow> ReadDataRows(Action<int, string[], Exception> onRowReadError = null)
        {
            var datatable = CsvFileDefinition.CreateDataTable(_definition);
            var stream = new StreamReader(_path);
            // skip header. TODO: header parsing
            for (var i = 0; i < _definition.Header.NumRows; i++)
                stream.ReadLine();

            var rownum = 1 + _definition.Header.NumRows;
            var reader = new CsvReader(stream, CreateCsvConfiguration(_definition));
            try
            {
                while (reader.Read())
                {
                    var row = datatable.NewRow();
                    try
                    {
                        foreach (DataColumn col in datatable.Columns)
                            row[col.ColumnName] = reader[col.ColumnName];
                        rownum++;
                    }
                    catch (ArgumentException e)
                    {
                        if (onRowReadError != null)
                            onRowReadError(rownum, reader.CurrentRecord, e);
                        else
                            throw;
                    }
                    yield return row;
                }
            }
            finally
            {
                reader.Dispose();
                stream.Dispose();
                datatable.Dispose();
            }
        }

        private static CsvConfiguration CreateCsvConfiguration(CsvFileDefinition definition)
        {
            return new CsvConfiguration
            {
                Delimiter = definition.Options.Delimiter,
                HasHeaderRecord = definition.Options.FirstRowContainsColumnNames
            };
        }
    }
}
