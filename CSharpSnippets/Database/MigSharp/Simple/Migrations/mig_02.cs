using MigSharp;
using System.Data;

namespace CSharpSnippets.Database.MigSharp.Migrations.Simple
{
    [MigrationExport]
    class mig_02 : IReversibleMigration
    {
        public void Up(IDatabase db)
        {
            db.Tables["Customers"].AddNullableColumn("Age", DbType.Int32);
        }

        public void Down(IDatabase db)
        {
            db.Tables["Customers"].Columns["Age"].Drop();
        }
    }
}
