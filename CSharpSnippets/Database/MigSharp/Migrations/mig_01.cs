using MigSharp;
using System.Data;

namespace CSharpSnippets.Database.MigSharp.Migrations
{
    [MigrationExport(Tag = "create initial database")]
    class mig_01 : IReversibleMigration
    {
        public void Up(IDatabase db)
        {
            db.CreateTable("Customers")
              .WithPrimaryKeyColumn("Id", DbType.Int32).AsIdentity()
              .WithNotNullableColumn("Name", DbType.String).OfSize(255);
        }

        public void Down(IDatabase db)
        {
            db.Tables["Customers"].Drop();
        }
    }
}
