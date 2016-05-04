using NUnit.Framework;
using System;
using System.Data;
using Uozu.Utils.Database;

namespace UozuUtils.Test.Database
{
    [TestFixture]
    class SqlMapperTest
    {
        private DataTable _testData;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _testData = new DataTable();
            _testData.Columns.Add(new DataColumn { ColumnName = "id", AutoIncrement = true, AllowDBNull = false, DataType = typeof(int), Unique = true });
            _testData.Columns.Add(new DataColumn { ColumnName = "otherId", DataType = typeof(Guid), AllowDBNull = true });
            _testData.Columns.Add(new DataColumn { ColumnName = "myBit", DataType = typeof(bool), AllowDBNull = true });
            _testData.Columns.Add(new DataColumn { ColumnName = "myBigInt", DataType = typeof(long), AllowDBNull = true });
            _testData.Columns.Add(new DataColumn { ColumnName = "myTinyInt", DataType = typeof(byte), AllowDBNull = true });
            _testData.Columns.Add(new DataColumn { ColumnName = "mySmallInt", DataType = typeof(short), AllowDBNull = true });
            _testData.Columns.Add(new DataColumn { ColumnName = "myNullableInt", DataType = typeof(short), AllowDBNull = true });
            _testData.Columns.Add(new DataColumn { ColumnName = "myFloat", DataType = typeof(float), AllowDBNull = true });
            _testData.Columns.Add(new DataColumn { ColumnName = "myNvarchar", DataType = typeof(string), AllowDBNull = true });
            _testData.Columns.Add(new DataColumn { ColumnName = "myDateTime", DataType = typeof(DateTime), AllowDBNull = true, DefaultValue = DateTime.Now });

            for (var i = 0; i < 1; i++)
            {
                var row = _testData.NewRow();
                row["otherId"] = Guid.NewGuid();
                row["myBit"] = i % 2 == 0;
                row["myBigInt"] = (long)i;
                row["myTinyInt"] = (byte)i;
                row["mySmallInt"] = (short)i;
                row["myNullableInt"] = DBNull.Value;
                row["myFloat"] = (float)i;
                row["myNvarchar"] = i.ToString();
                _testData.Rows.Add(row);
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

        [Test]
        public void MapObjectWithReflection()
        {
            var reader = new MockDataReader(_testData);
            reader.Read();
            var model = SqlMapper.MapObjectWithReflection<SqlMapperTestModel>(reader);
            AssertFirstModel(model);
        }

        [Test]
        public void MapObjectWithCachedReflection()
        {
            var reader = new MockDataReader(_testData);
            reader.Read();
            var model = SqlMapper.MapObjectWithCachedReflection<SqlMapperTestModel>(reader);
            AssertFirstModel(model);
        }

        private void AssertFirstModel(SqlMapperTestModel model)
        {
            Assert.AreEqual(0, model.id);
            Assert.AreEqual(true, model.myBit.Value);
            Assert.AreEqual(0, model.myBigInt);
            Assert.AreEqual(0, model.myTinyInt);
            Assert.AreEqual(0, model.mySmallInt);
            Assert.AreEqual(null, model.myNullableInt);
            Assert.AreEqual(0, model.myFloat);
            Assert.AreEqual("0", model.myNvarchar);
        }

        private class IncorrectModel
        {
            public int id { get; set; }
            public Guid otherIdWrongName { get; set; }
        }

        [Test]
        public void IncorrectModelTest()
        {
            // no problem here - reader throws the error with unknown column name
            var reader = new MockDataReader(_testData);
            reader.Read();
            Assert.That(() => SqlMapper.MapObjectWithCachedReflection<IncorrectModel>(reader),
                Throws.ArgumentException);
        }

        private class IncorrectModelNonNullable
        {
            public int id { get; set; }
            public int myNullableInt { get; set; }
        }

        [Test]
        public void TryToMapNullableToNonNullable()
        {
            var reader = new MockDataReader(_testData);
            reader.Read();
            Assert.Throws(typeof(NullReferenceException),
                () => SqlMapper.MapObjectWithCachedReflection<IncorrectModelNonNullable>(reader));
        }

        [Test]
        public void TryToMapNullableToNonNullable_Reflection()
        {
            var reader = new MockDataReader(_testData);
            reader.Read();
            Assert.That(() => SqlMapper.MapObjectWithReflection<IncorrectModelNonNullable>(reader),
                Throws.ArgumentException);
        }

        private class SimpleObj
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        [Test]
        public void GetInsertCmd()
        {
            var cmd = SqlMapper.GetInsertCmdWithReflection("myTable", new SimpleObj { id = 1, name = "bert" });
            Assert.AreEqual("insert into [myTable] (id,name) values (@id,@name);", cmd);
        }
    }
}
