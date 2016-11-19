using MigSharp;
using System.Data;

namespace CSharpSnippets.Database.MigSharp.MultiModule.Module1.Migrations
{
    [MigrationExport(ModuleName = "Module1")]
    class mig_01 : IReversibleMigration
    {
        public void Up(IDatabase db)
        {
            db.CreateTable("Module1Table")
              .WithPrimaryKeyColumn("Id", DbType.Int32).AsIdentity()
              .WithNotNullableColumn("Name", DbType.String).OfSize(255);
        }

        public void Down(IDatabase db)
        {
            db.Tables["Module1Table"].Drop();
        }
    }
}
