using CSharpSnippets.Database;
using NUnit.Framework;
using System;
using System.Data.SqlClient;

namespace CSharpSnippets.Test.SqlMapperTests
{
    [TestFixture]
    public class SqlMapperTest
    {
        public const string TestTable = "SqlMapperTestModel";
        public const string ConnString =
            @"Data Source=localhost\sqlexpress2014;Initial Catalog=test1;Integrated Security=SSPI;";
        public const int NumTestRecords = 1;

        [OneTimeSetUp]
        public void Setup()
        {
            CreateTestTable();
            SeedTestData();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            ExecuteNonQuery(ConnString, $"drop table {TestTable}");
        }

        [Test]
        public void MapObjectWithReflection()
        {
            using (var con = new SqlConnection(ConnString))
            {
                var cmd = con.CreateCommand();
                con.Open();
                cmd.CommandText = $"select top(1) * from {TestTable}";
                var reader = cmd.ExecuteReader();
                reader.Read();
                var model = SqlMapper.MapObjectWithReflection<SqlMapperTestModel>(reader);
                reader.Close();
                Assert.AreEqual(1, model.id);
                Assert.AreEqual(false, model.myBit.Value);
                Assert.AreEqual(0, model.myBigInt);
                Assert.AreEqual(0, model.myTinyInt);
                Assert.AreEqual(0, model.mySmallInt);
                Assert.AreEqual(0, model.myFloat);
                Assert.AreEqual("0", model.myNvarchar);
                Assert.AreEqual("f", model.myChar);
            }
        }

        [Test]
        public void MapObjectWithCachedReflection()
        {
            using (var con = new SqlConnection(ConnString))
            {
                var cmd = con.CreateCommand();
                con.Open();
                cmd.CommandText = $"select top(1) * from {TestTable}";
                var reader = cmd.ExecuteReader();
                reader.Read();
                var model = SqlMapper.MapObjectWithCachedReflection<SqlMapperTestModel>(reader);
                reader.Close();
                Assert.AreEqual(1, model.id);
                Assert.AreEqual(false, model.myBit.Value);
                Assert.AreEqual(0, model.myBigInt);
                Assert.AreEqual(0, model.myTinyInt);
                Assert.AreEqual(0, model.mySmallInt);
                Assert.AreEqual(0, model.myFloat);
                Assert.AreEqual("0", model.myNvarchar);
                Assert.AreEqual("f", model.myChar);
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
            public double? myFloat { get; set; }
            public string myNvarchar { get; set; }
            public string myChar { get; set; }
            public DateTime? myDateTime { get; set; }
        }

        private void CreateTestTable()
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

        private void SeedTestData()
        {
            for (var i = 0; i < NumTestRecords; i++)
            {
                ExecuteNonQuery(ConnString,
                    $@"insert into {TestTable} (myBit, myBigInt, myTinyInt, mySmallInt, myFloat, myNvarchar, myChar)
                    values ({i % 2}, {i}, {i % 256}, {i % 65536}, {i}, '{i.ToString()}', 'f')");
            }
        }

        private void ExecuteNonQuery(string connString, string cmdText)
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
