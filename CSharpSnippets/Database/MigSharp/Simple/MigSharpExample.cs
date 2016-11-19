using MigSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CSharpSnippets.Database.MigSharp.Simple
{
    class MigSharpSimpleExample
    {
        private static Assembly _assembly = Assembly.GetExecutingAssembly();
        private static string _connectionString;
        private static Migrator _standardMigrator;

        public static void Run(string connectionString)
        {
            _connectionString = connectionString;
            _standardMigrator = new Migrator(connectionString, DbPlatform.SqlServer2014);
            MigrateAll();
            // MigrateTo(2);
            // ScriptTo(@"C:\temp\migsharpexample");
        }

        private static void MigrateAll()
        {
            Console.WriteLine("Updating to latest db version");
            var migrations = _standardMigrator.FetchMigrations(_assembly);
            Console.WriteLine($"{migrations.ScheduledMigrations.Count} pending migrations");
            _standardMigrator.MigrateAll(_assembly);
            Console.WriteLine("Done");
        }

        private static void MigrateTo(long timestamp)
        {
            Console.WriteLine("Migrating to db migration " + timestamp);
            var migrations = _standardMigrator.FetchMigrationsTo(_assembly, timestamp);
            if (migrations.ScheduledMigrations.First().Direction == MigrationDirection.Down)
                Console.WriteLine("Migration direction = down");
            Console.WriteLine($"{migrations.ScheduledMigrations.Count} pending migrations");
            migrations.Execute();
            Console.WriteLine("Done");
        }

        private static void ScriptTo(string dir, long timestamp = -1)
        {
            Directory.CreateDirectory(dir);
            var options = new MigrationOptions();
            options.OnlyScriptSqlTo(dir);
            var migrator = new Migrator(_connectionString, DbPlatform.SqlServer2014, options);
            IMigrationBatch migrations = null;
            if (timestamp != -1)
                migrations = migrator.FetchMigrationsTo(_assembly, timestamp);
            else
                migrations = migrator.FetchMigrations(_assembly);
            if (migrations.ScheduledMigrations.First().Direction == MigrationDirection.Down)
                Console.WriteLine("Migration direction = down");
            Console.WriteLine($"{migrations.ScheduledMigrations.Count} pending migrations");
            Console.WriteLine("Writing migration SQL scripts to {dir}");
            migrations.Execute();
            Console.WriteLine("Done");
        }
    }
}
