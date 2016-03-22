using System;
using System.Data.SqlClient;

namespace CSharpSnippets.Database
{
    class SqlClientExamples
    {
        public static void DataReaderExample()
        {
            // assumes you've got Northwind
            using (var connection = new SqlConnection(@"Data Source=localhost\sqlexpress2014;Initial Catalog=Northwind;Integrated Security=SSPI;"))
            {
                SqlCommand command = new SqlCommand("SELECT CategoryID, CategoryName FROM Categories;", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
            }
        }
    }
}
