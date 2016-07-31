using System;

namespace CSharpSnippets.Database.TestDb.Models
{
    class PersonModel
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Intro { get; set; }
    }
}
