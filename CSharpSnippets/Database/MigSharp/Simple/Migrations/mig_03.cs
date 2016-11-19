using MigSharp;
using System.Data;

namespace CSharpSnippets.Database.MigSharp.Migrations.Simple
{
    [MigrationExport]
    class mig_03 : IReversibleMigration
    {
        public void Up(IDatabase db)
        {
            // NOTE: this will throw a SQL exception if there is existing data > 100 characters
            db.Tables["Customers"].Columns["Name"].AlterToNotNullable(DbType.String).OfSize(100);
        }

        public void Down(IDatabase db)
        {
            db.Tables["Customers"].Columns["Name"].AlterToNotNullable(DbType.String).OfSize(255);
        }
    }
}
