using CSharpSnippets.Database.TestDb;
using System.Data;

namespace CSharpSnippets.Database
{
    class TransactionExamples
    {
        public static void Run()
        {
            new TestDbApi().RecreateObjects();
            asdf();
        }

        // this times out rather than deadlocks. Why? Same application, same connection?
        // try separate threads
        public static void asdf()
        {
            IDbConnection con1;
            IDbTransaction trans1;
            var cmd1 =
                @"insert into SimpleObject values (1, 'a');
                select * from SimpleObject where id < 10;";
            DoThing(cmd1, out con1, out trans1);

            IDbConnection con2;
            IDbTransaction trans2;
            var cmd2 =
                @"insert into SimpleObject values (1, 'a');
                select * from SimpleObject where id < 10;";
            DoThing(cmd2, out con2, out trans2);

            trans1.Commit();
            trans2.Commit();
            con1.Close();
            con2.Close();
        }

        private static void DoThing(string cmd, out IDbConnection con, out IDbTransaction trans)
        {
            con = TestDbApi.CreateOpenConnection();
            trans = con.BeginTransaction(IsolationLevel.Serializable);
            var sqlCmd = con.CreateCommand();
            sqlCmd.CommandText = cmd;
            sqlCmd.Transaction = trans;
            sqlCmd.ExecuteNonQuery();
        }
    }
}
