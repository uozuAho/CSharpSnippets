using MigSharp;
using System.Data;

namespace MigSharpExistingDbMigrations
{
    [MigrationExport(Tag = "add BlahTable")]
    public class Migration_201704042131 : IReversibleMigration
    {
        public void Up(IDatabase db)
        {
            db.CreateTable("BlahTable")
              .WithPrimaryKeyColumn("Id", DbType.Int32).AsIdentity()
              .WithNotNullableColumn("Name", DbType.String).OfSize(255);
        }

        public void Down(IDatabase db)
        {
            db.Tables["BlahTable"].Drop();
        }
    }
}
