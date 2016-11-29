using MigSharp;
using System;
using System.Reflection;

namespace CSharpSnippets.Database.MigSharp.MultiDb
{
    class MigSharpMultiDbExample
    {
        public static void Run(string connectionString1, string connectionString2)
        {
            MigrateAssembly(connectionString1, Assembly.GetExecutingAssembly());
            MigrateAssembly(connectionString2, Assembly.GetAssembly(typeof(OtherMigrations.mig_01)));
        }

        private static void MigrateAssembly(string connectionString, Assembly assembly)
        {
            var migrator = new Migrator(connectionString, DbPlatform.SqlServer2014);
            Console.WriteLine($"Updating to latest db version, assembly: {assembly.GetName().Name}");
            var migrations = migrator.FetchMigrations(assembly);
            Console.WriteLine($"{migrations.Steps.Count} pending migrations");
            migrator.MigrateAll(assembly);
            Console.WriteLine("Done");
        }
    }
}
