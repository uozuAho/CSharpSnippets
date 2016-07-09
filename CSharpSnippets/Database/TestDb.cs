using System.Data.SqlClient;

namespace CSharpSnippets.Database
{
    class TestDb
    {
        public const string DbName = "CSharpSnippets";
        public const string ConnStringMaster = @"Data Source=localhost\sqlexpress2014;Initial Catalog=master; Integrated Security=SSPI;";
        public const string ConnStringTest = @"Data Source=localhost\sqlexpress2014;Initial Catalog="+ DbName +"; Integrated Security=SSPI;";

        public static void DropAndCreate()
        {
            DropAndCreateDb();
            CreateTables(ConnStringTest);
        }

        private static void DropAndCreateDb()
        {
            ExecuteNonQuery(ConnStringMaster,
                "if exists (select * from sys.databases where name='" + DbName + "') " +
                "drop database " + DbName + "; " +
                "create database " + DbName + ";");
        }

        private static void CreateTables(string connstring)
        {
            ExecuteNonQuery(connstring,
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

        private static void ExecuteNonQuery(string connstring, string cmdtext)
        {
            using (var con = new SqlConnection(connstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = cmdtext;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }
    }
}
