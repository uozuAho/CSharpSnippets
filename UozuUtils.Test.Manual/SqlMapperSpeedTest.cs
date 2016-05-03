using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Uozu.Utils.Database;

namespace Uozu.Utils.Test.Manual
{
    class SqlMapperSpeedTest
    {
        private const int NumTestRecords = 50000;

        // use the profiler to see run times of each of the mapping methods. Stopwatch is misleading...
        public static void Run()
        {
            var testTable = CreateTestTable();

            var stopwatch = new Stopwatch();
            var models = new List<SqlMapperTestModel>();
            var reader = new MockDataReader(testTable);
            stopwatch.Start();
            while (reader.Read())
                models.Add(SqlMapper.MapObjectWithReflection<SqlMapperTestModel>(reader));
            stopwatch.Stop();
            Console.WriteLine($"Using pure reflection: read {NumTestRecords} in {stopwatch.ElapsedMilliseconds} ms");

            models.Clear();
            reader = new MockDataReader(testTable);
            stopwatch.Reset();
            stopwatch.Start();
            while (reader.Read())
                models.Add(SqlMapper.MapObjectWithCachedReflection<SqlMapperTestModel>(reader));
            stopwatch.Stop();
            Console.WriteLine($"Using cached expressions: read {NumTestRecords} in {stopwatch.ElapsedMilliseconds} ms");

            models.Clear();
            reader = new MockDataReader(testTable);
            stopwatch.Reset();
            stopwatch.Start();
            while (reader.Read())
                models.Add(MapModelHardCoded(reader));
            stopwatch.Stop();
            Console.WriteLine($"Using hard coded: read {NumTestRecords} in {stopwatch.ElapsedMilliseconds} ms");
        }

        private static SqlMapperTestModel MapModelHardCoded(IDataReader reader)
        {
            var m = new SqlMapperTestModel();
            m.id = reader.GetInt32(0);
            m.otherId = reader.GetGuid(1);
            m.myBit = reader.GetBoolean(2);
            m.myBigInt = reader.GetInt64(3);
            m.myTinyInt = reader.GetByte(4);
            m.mySmallInt = reader.GetInt16(5);
            var temp = reader["myNullableInt"];
            m.myNullableInt = temp == null || temp == DBNull.Value ? null : (int?)temp;
            m.myFloat = reader.GetFloat(7);
            m.myNvarchar = reader.GetString(8);
            m.myDateTime = reader.GetDateTime(9);
            return m;
        }

        private class SqlMapperTestModel
        {
            public int id { get; set; }
            public Guid? otherId { get; set; }
            public bool? myBit { get; set; }
            public long? myBigInt { get; set; }
            public byte? myTinyInt { get; set; }
            public short? mySmallInt { get; set; }
            public int? myNullableInt { get; set; }
            public float? myFloat { get; set; }
            public string myNvarchar { get; set; }
            public DateTime? myDateTime { get; set; }
        }

        private static DataTable CreateTestTable()
        {
            var table = new DataTable("testTable");
            table.Columns.Add(new DataColumn { ColumnName = "id", AutoIncrement = true, AllowDBNull = false, DataType = typeof(int), Unique = true });
            table.Columns.Add(new DataColumn { ColumnName = "otherId", DataType = typeof(Guid), AllowDBNull = true });
            table.Columns.Add(new DataColumn { ColumnName = "myBit", DataType = typeof(bool), AllowDBNull = true });
            table.Columns.Add(new DataColumn { ColumnName = "myBigInt", DataType = typeof(long), AllowDBNull = true });
            table.Columns.Add(new DataColumn { ColumnName = "myTinyInt", DataType = typeof(byte), AllowDBNull = true });
            table.Columns.Add(new DataColumn { ColumnName = "mySmallInt", DataType = typeof(short), AllowDBNull = true });
            table.Columns.Add(new DataColumn { ColumnName = "myNullableInt", DataType = typeof(int), AllowDBNull = true });
            table.Columns.Add(new DataColumn { ColumnName = "myFloat", DataType = typeof(float), AllowDBNull = true });
            table.Columns.Add(new DataColumn { ColumnName = "myNvarchar", DataType = typeof(string), AllowDBNull = true });
            table.Columns.Add(new DataColumn { ColumnName = "myDateTime", DataType = typeof(DateTime), AllowDBNull = true, DefaultValue = DateTime.Now });

            SeedTestData(table);
            return table;
        }

        private static void SeedTestData(DataTable table)
        {
            for (var i = 0; i < NumTestRecords; i++)
            {
                var row = table.NewRow();
                row["otherId"] = Guid.NewGuid();
                row["myBit"] = i % 2 == 0;
                row["myBigInt"] = (long)i;
                row["myTinyInt"] = (byte)i;
                row["mySmallInt"] = (short)i;
                row["myFloat"] = (float)i;
                row["myNvarchar"] = i.ToString();
                table.Rows.Add(row);
            }
        }
    }
}
