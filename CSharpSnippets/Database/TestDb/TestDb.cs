﻿using System;
using System.Data;
using System.Data.SqlClient;
using Uozu.Utils.Database;

namespace CSharpSnippets.Database.TestDb
{
    class TestDbApi
    {
        public const string DbName = "CSharpSnippets";
        public const string ConnStringMaster = @"Data Source=localhost\sql2016;Initial Catalog=master; Integrated Security=SSPI;";
        public const string ConnStringTest = @"Data Source=localhost\sql2016;Initial Catalog=" + DbName + "; Integrated Security=SSPI;";

        private IDbApi _testDb;

        public Person Person;

        public TestDbApi()
        {
            _testDb = new SqlDbApi(ConnStringTest);
            Init();
        }

        private void Init()
        {
            Person = new Person(_testDb);
        }

        public void ExecuteNonQuery(string cmd)
        {
            _testDb.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Drop entire db and recreate
        /// </summary>
        public void DropDbAndCreateAll()
        {
            Console.WriteLine("dropping and recreating test db...");
            DropAndCreateDb();
            CreateTables();
            CreateStoredProcs();
        }

        /// <summary>
        /// Drop and create all tables, stored procs etc.
        /// </summary>
        public void RecreateObjects()
        {
            Console.WriteLine("dropping and recreating test db objects...");
            DropTables();
            DropStoredProcs();
            CreateTables();
            CreateStoredProcs();
        }

        public static IDbConnection CreateOpenConnection()
        {
            var con = new SqlConnection(ConnStringTest);
            con.Open();
            return con;
        }

        private static void DropAndCreateDb()
        {
            var master = new SqlDbApi(ConnStringMaster);
            master.ExecuteNonQuery(
                "if exists (select * from sys.databases where name='" + DbName + "') " +
                "drop database " + DbName + "; " +
                "create database " + DbName + ";");
        }

        private void DropTables()
        {
            _testDb.ExecuteNonQuery(
@"drop table [dbo].[Person];
drop table [dbo].[Person2];
drop table [dbo].[SimpleObject];");
        }

        private void CreateTables()
        {
            _testDb.ExecuteNonQuery(
@"CREATE TABLE [dbo].[Person] (
    [id][int] IDENTITY(1, 1) PRIMARY KEY CLUSTERED NOT NULL,
    [FirstName] [varchar](100) NOT NULL,
    [LastName] [varchar](100) NOT NULL,
    [DateOfBirth] [datetime] NOT NULL,
    [Intro] [ntext] NULL
);

CREATE TABLE [dbo].[Person2] (
    [id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED NOT NULL,
    [FirstName] [varchar](100) NOT NULL,
    [LastName] [varchar](100) NOT NULL,
    [DateOfBirth] [datetime] NOT NULL,
    [NumCats] [nvarchar](10) NULL,
    [Intro] [ntext] NULL
);

CREATE TABLE [dbo].[SimpleObject] (
    [id] [int] NOT NULL,
    [name] [nvarchar](50) NULL
);");
        }

        private void DropStoredProcs()
        {
            _testDb.ExecuteNonQuery(
@"drop procedure ExampleProc;");
        }

        private void CreateStoredProcs()
        {
            _testDb.ExecuteNonQuery(
@"CREATE PROCEDURE ExampleProc
    @name nvarchar(30),
    @number int
AS
    SELECT @name, @number, 'something else';");
        }
    }
}
