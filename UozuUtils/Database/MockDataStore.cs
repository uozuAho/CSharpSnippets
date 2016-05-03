using System.Collections.Generic;
using System.Data;

namespace Uozu.Utils.Database
{
    public class MockDataStore
    {
        private Dictionary<string, DataTable> _tables;

        public void AddDataTable(DataTable table)
        {
            _tables[table.TableName] = table;
        }

        public IDataReader GetReader(string tableName)
        {
            return new MockDataReader(_tables[tableName]);
        }
    }
}
