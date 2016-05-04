using System;
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
    }
}
