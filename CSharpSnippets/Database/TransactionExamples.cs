using CSharpSnippets.Database.TestDb;
using CSharpSnippets.Database.TestDb.Models;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpSnippets.Database
{
    class TransactionExamples
    {
        public static void Run()
        {
            new TestDbApi().RecreateObjects();
            Deadlock();
        }

        public static void Deadlock()
        {
            var testDb = new TestDbApi();
            testDb.Person.Insert(new PersonModel { FirstName = "person1", LastName = "asdf", DateOfBirth = DateTime.Now});
            testDb.Person.Insert(new PersonModel { FirstName = "person2", LastName = "asdf", DateOfBirth = DateTime.Now });
            testDb.ExecuteNonQuery("insert into Person2 (FirstName, LastName, DateOfBirth) values ('person1', 'asdf', '2016-01-01');");
            testDb.ExecuteNonQuery("insert into Person2 (FirstName, LastName, DateOfBirth) values ('person2', 'asdf', '2016-01-01');");

            var cmd1 =
                @"update Person set FirstName = 'person1 updated1' where FirstName = 'person1';
                waitfor delay '00:00:01';
                update Person2 set FirstName = 'person2 updated1' where FirstName = 'person2';";
            var cmd2 =
                @"update Person2 set FirstName = 'person2 updated2' where FirstName = 'person2';
                waitfor delay '00:00:01';
                update Person set FirstName = 'person1 updated2' where FirstName = 'person1';";

            Parallel.Invoke(
                () => ExecuteInTransaction(cmd1, 0),
                () => ExecuteInTransaction(cmd2, 0));
        }

        private static void ExecuteInTransaction(string cmd, int preCommitDelayMs)
        {
            using (var con = TestDbApi.CreateOpenConnection())
            {
                using (var trans = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    var sqlCmd = con.CreateCommand();
                    sqlCmd.CommandText = cmd;
                    sqlCmd.Transaction = trans;
                    sqlCmd.ExecuteNonQuery();
                    Thread.Sleep(preCommitDelayMs);
                    trans.Commit();
                }
            }
        }
    }
}
