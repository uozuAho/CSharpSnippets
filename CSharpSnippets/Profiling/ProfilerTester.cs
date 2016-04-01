using System;
using System.Data.SqlClient;
using System.Threading;

namespace CSharpSnippets.Profiling
{
    class ProfilerTester
    {
        public static void Run()
        {
            HeavyProcessingLoad();
            LargeMemGrab();
            LongRunningIdle();
            LongRunningSql();
        }

        private static void HeavyProcessingLoad()
        {
            Console.WriteLine("HeavyProcessingLoad");
            var random = new Random();
            for (int i = 0; i < 1000000; i++)
            {
                var a = Math.Sqrt(random.NextDouble());
            }
        }

        private static void LongRunningIdle()
        {
            Console.WriteLine("LongRunningIdle");
            Thread.Sleep(2000);
        }

        private static void LargeMemGrab()
        {
            Console.WriteLine("LargeMemGrab");
            var asdf = new int[100000000];
        }

        private static void LongRunningSql()
        {
            Console.WriteLine("LongRunningSql");
            // assumes you've got Northwind
            using (var connection = new SqlConnection(
                @"Data Source=localhost\sqlexpress2014;Initial Catalog=Northwind;Integrated Security=SSPI;"))
            {
                SqlCommand command = new SqlCommand("WAITFOR DELAY '00:00:05'", connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
