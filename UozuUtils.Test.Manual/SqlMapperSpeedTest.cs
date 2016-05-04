using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Uozu.Utils.Database;

namespace Uozu.Utils.Test.Manual
{
    class SqlMapperSpeedTest
    {
        private const int NumTestRecords = 200000;

        // use the profiler to see run times of each of the mapping methods. Stopwatch is misleading...
        public static void Run()
        {
            var testTable = CreateTestTable();

            var stopwatch = new Stopwatch();
            var models = new List<SqlMapperTestModel>();
            var reader = new MockDataReader(testTable);
            stopwatch.Start();
            while (reader.Read())
                models.Add(MapModelHardCoded(reader));
            stopwatch.Stop();
            var hardCodedMs = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Using hard coded: read {NumTestRecords} in {hardCodedMs} ms");

            models.Clear();
            reader = new MockDataReader(testTable);
            stopwatch.Reset();
            stopwatch.Start();
            while (reader.Read())
                models.Add(SqlMapper.MapObjectWithCachedReflection<SqlMapperTestModel>(reader));
            stopwatch.Stop();
            var timesHardCodedMs = (float) stopwatch.ElapsedMilliseconds / hardCodedMs;
            Console.WriteLine($"Using cached expressions: read {NumTestRecords} in {stopwatch.ElapsedMilliseconds} ms ({timesHardCodedMs}x) ");

            models.Clear();
            reader = new MockDataReader(testTable);
            stopwatch.Reset();
            stopwatch.Start();
            while (reader.Read())
                models.Add(SqlMapper.MapObjectWithReflection<SqlMapperTestModel>(reader));
            stopwatch.Stop();
            timesHardCodedMs = (float)stopwatch.ElapsedMilliseconds / hardCodedMs;
            Console.WriteLine($"Using pure reflection: read {NumTestRecords} in {stopwatch.ElapsedMilliseconds} ms ({timesHardCodedMs}x)");
        }

        /// <summary>
        /// Map a record to a SqlMapperTestModel, in the same way as the compiled expression in SqlMapper
        /// </summary>
        private static SqlMapperTestModel MapModelHardCoded(IDataRecord record)
        {
            var m = new SqlMapperTestModel();
            m.id = GetField<int>(record, "id");
            m.otherId = GetField<Guid>(record, "otherId");
            m.myBit = GetField<bool>(record, "myBit");
            m.myBigInt = GetField<long>(record, "myBigInt");
            m.myTinyInt = GetField<byte>(record, "myTinyInt");
            m.mySmallInt = GetField<short>(record, "mySmallInt");
            m.myNullableInt = GetField<int?>(record, "myNullableInt");
            m.myFloat = GetField<float>(record, "myFloat");
            m.myNvarchar = GetField<string>(record, "myNvarchar");
            m.myDateTime = GetField<DateTime>(record, "myDateTime");
            return m;
        }

        private static T GetField<T>(IDataRecord record, string fieldName)
        {
            try
            {
                if (record[fieldName] == DBNull.Value)
                    return default(T); 
                return (T) Convert.ChangeType(record[fieldName], typeof(T));
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException(fieldName);
            }
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
