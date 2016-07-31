using CSharpSnippets.Database.TestDb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public int Insert(PersonModel person)
        {
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@fn", person.FirstName),
                new SqlParameter("@ln", person.LastName),
                new SqlParameter("@dob", person.DateOfBirth)
            };
            if (person.Intro == null)
                sqlParams.Add(new SqlParameter("@intro", DBNull.Value));
            else
                sqlParams.Add(new SqlParameter("@intro", person.Intro));
            return _db.ExecuteScalar<int>(
                "insert into Person (FirstName, LastName, DateOfBirth, Intro) " +
                "values (@fn, @ln, @dob, @intro); " +
                "select scope_identity();",
                sqlParams.ToArray());
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
