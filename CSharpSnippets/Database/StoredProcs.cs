using System;
using System.Collections.Generic;
using System.Data;

namespace CSharpSnippets.Database
{
    class StoredProcs
    {
        public static void Run()
        {
            TestDb.DropAndCreate();
            Console.WriteLine("Results from stored proc:");
            foreach (var m in ExecuteExampleProc())
            {
                Console.WriteLine(m);
            }
        }

        // TODO: use SqlDbApi.ExecuteReader for this
        private static IEnumerable<ExampleProcModel> ExecuteExampleProc()
        {
            using (var con = TestDb.CreateOpenConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "ExampleProc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        yield return DeserialiseExampleProcModel(reader);
                    }
                }
            }
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
