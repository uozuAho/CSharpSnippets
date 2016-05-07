using System;
using System.Data;
using System.Data.SqlClient;

namespace CSharpSnippets.Database
{
    class SqlClientExamples
    {
        public static void DataReaderExample()
        {
            // assumes you've got Northwind
            using (var con = new SqlConnection(@"Data Source=localhost\sqlexpress2014;Initial Catalog=Northwind;Integrated Security=SSPI;"))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT CategoryID, CategoryName FROM Categories where CategoryID < @id;", con);
                cmd.Parameters.AddWithValue("@id", 4);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                }
                else
                    Console.WriteLine("No rows found.");
                reader.Close();
            }
        }

        public static void BulkCopyDataTableExamples()
        {
            //SimpleBulkCopyDataTable();
            SimpleBulkCopyDataTableImplicitTypeConversion();
        }

        private static void SimpleBulkCopyDataTable()
        {
            var data = new DataTable();
            data.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "FirstName", AllowDBNull = false });
            data.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "LastName", MaxLength = 100 });
            data.Columns.Add(new DataColumn { DataType = typeof(DateTime), ColumnName = "DateOfBirth" });
            data.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "Intro" });

            int numRows = 10;
            for (var i = 0; i < numRows; i++)
            {
                var row = data.NewRow();
                row["FirstName"] = $"First Name {i}";
                row["LastName"] = $"Last Name {i}";
                row["DateOfBirth"] = DateTime.Now;
                if (i % 2 == 0) row["Intro"] = $"Intro {i}";
                data.Rows.Add(row);
            }

            using (var con = new SqlConnection(@"Data Source=localhost\sqlexpress2014;Initial Catalog=test1;Integrated Security=SSPI;"))
            {
                con.Open();
                using (var blk = new SqlBulkCopy(con))
                {
                    blk.DestinationTableName = "Person";
                    // have to add column mappings since we're not inserting the id column
                    blk.ColumnMappings.Add("FirstName", "FirstName");
                    blk.ColumnMappings.Add("LastName", "LastName");
                    blk.ColumnMappings.Add("DateOfBirth", "DateOfBirth");
                    blk.ColumnMappings.Add("Intro", "Intro");
                    blk.WriteToServer(data);
                }
            }
        }

        private static void SimpleBulkCopyDataTableImplicitTypeConversion()
        {
            var data = new DataTable();
            data.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "FirstName", AllowDBNull = false });
            data.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "LastName", MaxLength = 100 });
            data.Columns.Add(new DataColumn { DataType = typeof(DateTime), ColumnName = "DateOfBirth" });
            // 'NumCats' is a string in the DB, but an int here. The conversion happens automatically (.NET 4.6, SQL server 2014)
            data.Columns.Add(new DataColumn { DataType = typeof(int), ColumnName = "NumCats" });
            data.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "Intro" });

            int numRows = 10;
            for (var i = 0; i < numRows; i++)
            {
                var row = data.NewRow();
                row["FirstName"] = $"First Name {i}";
                row["LastName"] = $"Last Name {i}";
                row["DateOfBirth"] = DateTime.Now;
                if (i % 2 == 0) row["NumCats"] = i;
                if (i % 2 == 0) row["Intro"] = $"Intro {i}";
                data.Rows.Add(row);
            }

            using (var con = new SqlConnection(@"Data Source=localhost\sqlexpress2014;Initial Catalog=test1;Integrated Security=SSPI;"))
            {
                con.Open();
                using (var blk = new SqlBulkCopy(con))
                {
                    blk.DestinationTableName = "Person2";
                    // have to add column mappings since we're not inserting the id column
                    blk.ColumnMappings.Add("FirstName", "FirstName");
                    blk.ColumnMappings.Add("LastName", "LastName");
                    blk.ColumnMappings.Add("DateOfBirth", "DateOfBirth");
                    blk.ColumnMappings.Add("NumCats", "NumCats");
                    blk.ColumnMappings.Add("Intro", "Intro");
                    blk.WriteToServer(data);
                }
            }
        }
    }
}
