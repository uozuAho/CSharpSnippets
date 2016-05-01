using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CSharpSnippets.Database
{
    class SqlMapperSpeedTest
    {
        public const string TestTable = "SqlMapperTestModel";
        public const string ConnString =
            @"Data Source=localhost\sqlexpress2014;Initial Catalog=test1;Integrated Security=SSPI;";
        public const int NumTestRecords = 10000;

        // use the profiler to see run times of each of the mapping methods. Stopwatch is misleading...
        public static void Run()
        {
            CreateTestTable();
            SeedTestData();

            var stopwatch = new Stopwatch();
            var models = new List<SqlMapperTestModel>();
            using (var con = new SqlConnection(ConnString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"select * from {TestTable}";
                var reader = cmd.ExecuteReader();
                stopwatch.Start();
                while (reader.Read())
                {
                    models.Add(SqlMapper.MapObjectWithReflection<SqlMapperTestModel>(reader));
                }
                stopwatch.Stop();
            }
            Console.WriteLine($"Using pure reflection: read {NumTestRecords} in {stopwatch.ElapsedMilliseconds} ms");

            models.Clear();
            using (var con = new SqlConnection(ConnString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"select * from {TestTable}";
                var reader = cmd.ExecuteReader();
                stopwatch.Start();
                while (reader.Read())
                {
                    models.Add(SqlMapper.MapObjectWithCachedReflection<SqlMapperTestModel>(reader));
                }
                stopwatch.Stop();
            }
            Console.WriteLine($"Using cached expressions: read {NumTestRecords} in {stopwatch.ElapsedMilliseconds} ms");

            models.Clear();
            using (var con = new SqlConnection(ConnString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"select * from {TestTable}";
                var reader = cmd.ExecuteReader();
                stopwatch.Start();
                while (reader.Read())
                {
                    models.Add(MapModelHardCoded(reader));
                }
                stopwatch.Stop();
            }
            Console.WriteLine($"Using hard coded: read {NumTestRecords} in {stopwatch.ElapsedMilliseconds} ms");

            ExecuteNonQuery(ConnString, $"drop table {TestTable}");
        }

        private static SqlMapperTestModel MapModelHardCoded(SqlDataReader reader)
        {
            var m = new SqlMapperTestModel();
            m.id = reader.GetInt32(0);
            m.otherId = reader.GetGuid(1);
            m.myBit = reader.GetBoolean(2);
            m.myBigInt = reader.GetInt64(3);
            m.myTinyInt = reader.GetByte(4);
            m.mySmallInt = reader.GetInt16(5);
            m.myFloat = reader.GetDouble(6);
            m.myNvarchar = reader.GetString(7);
            m.myChar = reader.GetString(8);
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
            public double? myFloat { get; set; }
            public string myNvarchar { get; set; }
            public string myChar { get; set; }
            public DateTime? myDateTime { get; set; }
        }

        private static void CreateTestTable()
        {
            ExecuteNonQuery(ConnString,
                $@"create table {TestTable} (
                    id int primary key identity(1,1),
                    otherId uniqueidentifier default NEWID(),
                    myBit bit,
                    myBigInt bigint,
                    myTinyInt tinyint,
                    mySmallInt smallint,
                    myFloat float,
                    myNvarchar nvarchar(50),
                    myChar char,
                    myDateTime DateTime default GETDATE()
                );");
        }

        private static void SeedTestData()
        {
            for (var i = 0; i < NumTestRecords; i++)
            {
                ExecuteNonQuery(ConnString,
                    $@"insert into {TestTable} (myBit, myBigInt, myTinyInt, mySmallInt, myFloat, myNvarchar, myChar)
                    values ({i % 2}, {i}, {i % 256}, {i % 65536}, {i}, '{i.ToString()}', 'f')");
            }
        }

        private static void ExecuteNonQuery(string connString, string cmdText)
        {
            using (var con = new SqlConnection(connString))
            {
                var cmd = con.CreateCommand();
                cmd.CommandText = cmdText;
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
