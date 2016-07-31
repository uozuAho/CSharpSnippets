using CSharpSnippets.Database.TestDb.Models;
using System.Collections.Generic;
using System.Data;
using Uozu.Utils.Database;

namespace CSharpSnippets.Database.TestDb
{
    class Person
    {
        private IDbApi _db;

        public Person(IDbApi db)
        {
            _db = db;
        }

        public IEnumerable<PersonModel> SelectAll()
        {
            return _db.ExecuteReader("select * from Person", Deserialise);
        }

        // Example reader. Could also use SqlMapper.GetMapper
        private static PersonModel Deserialise(IDataRecord record)
        {
            return new PersonModel
            {
                id = record.GetInt32(0),
                FirstName = record.GetString(1),
                LastName = record.GetString(2),
                DateOfBirth = record.GetDateTime(3),
                Intro = record.IsDBNull(4) ? null : record.GetString(4)
            };
        }
    }
}
