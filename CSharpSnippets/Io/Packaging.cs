using System.IO;

namespace CSharpSnippets.Io
{
    
    public class Packaging
    {
        public static void PackagingExample()
        {
            var a = new A
            {
                Name = "A_name",
                MyB = new B { Name = "B name" },
                MyC = new C
                {
                    Name = "C name",
                    LastName = "C lastname"
                }
            };

            var fileStream = new FileStream(@"C:\temp\blahpack.pack", FileMode.Create);
            var packer = new ClassDataPacker(fileStream);
            packer.WriteEntity(a, x => x.Name);
            fileStream.Flush();
        }

        public class A
        {
            public string Name { get; set; }
            public B MyB { get; set; }
            public C MyC { get; set; }
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
}
