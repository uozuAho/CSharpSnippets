namespace UozuUtils.Test
{
    public class Config
    {
        public const string TestDbConnString =
            @"Data Source=localhost\sqlexpress2014;Initial Catalog=test1;Integrated Security=SSPI;";

        public const string CreateTestTablesCmd =
            @"IF OBJECT_ID('dbo.SimpleObject', 'U') IS NOT NULL DROP TABLE dbo.SimpleObject; 
            CREATE TABLE SimpleObject (
                id int not null,
                name nvarchar(50)
            );";
    }
}
