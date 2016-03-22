using Dapper;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace CSharpSnippets.Database
{
    public class DapperExamples
    {
        class Product
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public string asdfasdf { get; set; }
        }

        public static void SimpleRead()
        {
            // assumes you've got Northwind
            using (var connection = new SqlConnection(@"Data Source=localhost\sqlexpress2014;Initial Catalog=Northwind;Integrated Security=SSPI;"))
            {
                var products = connection.Query<Product>("select * from Products");

                Console.WriteLine("Num products: " + products.Count());
                Console.WriteLine("First product: " + products.First().ProductName);
                Console.WriteLine("asdfasdf: " + products.First().asdfasdf);
            }
        }
    }
}
