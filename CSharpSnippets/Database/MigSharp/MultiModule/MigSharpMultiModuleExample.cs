using MigSharp;
using System;
using System.Reflection;

namespace CSharpSnippets.Database.MigSharp.MultiModule
{
    class MigSharpMultiModuleExample
    {
        private static Assembly _assembly = Assembly.GetExecutingAssembly();
        private static string _connectionString;
        private static Migrator _standardMigrator;

        /// <summary>
        /// Note that this will run all migrations in the assembly, not just under this namespace
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Run(string connectionString)
        {
            _connectionString = connectionString;
            _standardMigrator = new Migrator(connectionString, DbPlatform.SqlServer2014);
            MigrateAll();
        }

        private static void MigrateAll()
        {
            Console.WriteLine("Updating to latest db version");
            var migrations = _standardMigrator.FetchMigrations(_assembly);
            Console.WriteLine($"{migrations.Steps.Count} pending migrations");
            _standardMigrator.MigrateAll(_assembly);
            Console.WriteLine("Done");
        }
    }
}
