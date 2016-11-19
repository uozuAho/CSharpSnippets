using MigSharp;
using System.Data;

namespace CSharpSnippets.Database.MigSharp.MultiModule.Module2.Migrations
{
    [MigrationExport(ModuleName = "Module2")]
    class mig_01 : IReversibleMigration
    {
        public void Up(IDatabase db)
        {
            db.CreateTable("Module2Table")
              .WithPrimaryKeyColumn("Id", DbType.Int32).AsIdentity()
              .WithNotNullableColumn("Name", DbType.String).OfSize(255);
        }

        public void Down(IDatabase db)
        {
            db.Tables["Module2Table"].Drop();
        }
    }
}
