using MigSharp;
using System.Data;

namespace OtherMigrations
{
    [MigrationExport(ModuleName = "OtherMigrations")]
    public class mig_01 : IReversibleMigration
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
