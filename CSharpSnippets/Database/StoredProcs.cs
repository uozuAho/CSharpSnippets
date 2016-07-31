using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Uozu.Utils.Database;
using CSharpSnippets.Database.TestDb;

namespace CSharpSnippets.Database
{
    class StoredProcs
    {
        public static void Run()
        {
            var db = new TestDbApi();
            db.DropDbAndCreateAll();
            Console.WriteLine("Results from stored proc:");
            foreach (var m in ExecuteExampleProc("Johnny", 5))
            {
                Console.WriteLine(m);
            }
        }

        private static IEnumerable<ExampleProcModel> ExecuteExampleProc(string name, int number)
        {
            IDbApi db = new SqlDbApi(TestDbApi.ConnStringTest);
            return db.ExecuteReader("ExampleProc", DeserialiseExampleProcModel, CommandType.StoredProcedure,
                new SqlParameter("@name", name),
                new SqlParameter("number", number));
        }

        private static ExampleProcModel DeserialiseExampleProcModel(IDataRecord record)
        {
            return new ExampleProcModel
            {
                name = record.GetString(0),
                number = record.IsDBNull(1) ? (int?) null : record.GetInt32(1),
                other = record.GetString(2)
            };
        }
    }

    class ExampleProcModel
    {
        public string name { get; set; }
        public int? number { get; set; }
        public string other { get; set; }

        public override string ToString()
        {
            return $"name: {name}, number: {number}, other: {other}";
        }
    }
}
