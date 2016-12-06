namespace CSharpSnippets.Serialization
{
    public class TestObj
    {
        public string Name { get; set; }
        public B MyB { get; set; }
        public C MyC { get; set; }
        public B MyBRef { get; set; }

        public static TestObj GetDefault()
        {
            var testObj = new TestObj
            {
                Name = "A_name",
                MyB = new B { Name = "B name" },
                MyC = new C
                {
                    Name = "C name",
                    LastName = "C lastname"
                },
            };
            testObj.MyBRef = testObj.MyB;
            return testObj;
        }
    }

    public class B
    {
        public string Name { get; set; }
    }

    public class C : B
    {
        public string LastName { get; set; }
    }
}
