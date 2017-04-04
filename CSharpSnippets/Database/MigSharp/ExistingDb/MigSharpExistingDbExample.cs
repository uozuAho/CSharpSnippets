using MigSharp;
using System;
using System.Data.SqlClient;
using System.Reflection;

namespace CSharpSnippets.Database.MigSharp.ExistingDb
{
    class MigSharpExistingDbExample
    {
        public static void Run()
        {
            var connectionString = @"Data Source = localhost\sql2016; Initial Catalog = CSharpSnippets; Integrated Security = SSPI;";
            MigrateAssembly(connectionString, Assembly.GetAssembly(typeof(MigSharpExistingDbMigrations.Migration_201704042131)));
        }

        private static void MigrateAssembly(string connectionString, Assembly assembly)
        {
            var migrationSelector = new MigrationSelector(connectionString);
            var migrator = new Migrator(connectionString, DbPlatform.SqlServer2014, new MigrationOptions
            {
                MigrationSelector = migrationSelector.Select
            });
            Console.WriteLine($"Updating to latest db version, assembly: {assembly.GetName().Name}");
            var migrations = migrator.FetchMigrations(assembly);
            Console.WriteLine($"{migrations.Steps.Count} pending migrations");
            migrator.MigrateAll(assembly);
            Console.WriteLine("Done");
        }

        /// <summary>
        /// Determines migrations to perform based on state of the target database
        /// </summary>
        private class MigrationSelector
        {
            private string _connectionString;
            private bool _skipFirstMigration = false;

            public MigrationSelector(string connectionString)
            {
                _connectionString = connectionString;
                _skipFirstMigration = TargetHasBlahTable();
            }

            public bool Select(IMigrationMetadata data)
            {
                if (_skipFirstMigration)
                    return data.Timestamp > 201704042131;
                else
                    return true;
            }

            private bool TargetHasBlahTable()
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandText = "if exists (select * from information_schema.tables where table_name = 'BlahTable') " +
                                      "select 1 else select 0;";
                    var result = (int) cmd.ExecuteScalar();
                    return result == 1;
                }
            }
        }
    }
}
