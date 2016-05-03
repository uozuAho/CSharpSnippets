using Uozu.Utils.Database;
using NUnit.Framework;
using System.Data;
using System;

namespace UozuUtils.Test
{
    class MockDataReaderTest
    {
        private MockDataReader _reader;

        private const string _tableName = "readerTable";
        private DataTable _readerTable;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _readerTable = new DataTable(_tableName);
            _readerTable.Columns.Add(new DataColumn { ColumnName = "id", AutoIncrement = true, AllowDBNull = false, DataType = typeof(int), Unique = true });
            _readerTable.Columns.Add(new DataColumn { ColumnName = "otherId", DataType = typeof(Guid), AllowDBNull = true });
            _readerTable.Columns.Add(new DataColumn { ColumnName = "myBit", DataType = typeof(bool), AllowDBNull = true });
            _readerTable.Columns.Add(new DataColumn { ColumnName = "myBigInt", DataType = typeof(long), AllowDBNull = true });
            _readerTable.Columns.Add(new DataColumn { ColumnName = "myTinyInt", DataType = typeof(byte), AllowDBNull = true });
            _readerTable.Columns.Add(new DataColumn { ColumnName = "mySmallInt", DataType = typeof(short), AllowDBNull = true });
            _readerTable.Columns.Add(new DataColumn { ColumnName = "myFloat", DataType = typeof(float), AllowDBNull = true });
            _readerTable.Columns.Add(new DataColumn { ColumnName = "myNvarchar", DataType = typeof(string), AllowDBNull = true });
            _readerTable.Columns.Add(new DataColumn { ColumnName = "myDateTime", DataType = typeof(DateTime), AllowDBNull = true, DefaultValue = DateTime.Now });

            for (var i = 0; i < 1; i++)
            {
                var row = _readerTable.NewRow();
                row["otherId"] = Guid.NewGuid();
                row["myBit"] = i % 2 == 0;
                row["myBigInt"] = (long)i;
                row["myTinyInt"] = (byte)i;
                row["mySmallInt"] = (short)i;
                row["myFloat"] = (float)i;
                row["myNvarchar"] = i.ToString();
                _readerTable.Rows.Add(row);
            }
        }

        [SetUp]
        public void Setup()
        {
            _reader = new MockDataReader(_readerTable);
        }

        [Test]
        public void ReadAllFields()
        {
            Assert.AreEqual(true, _reader.Read());
            Assert.AreEqual(0, _reader["id"]);
        }
    }
}
