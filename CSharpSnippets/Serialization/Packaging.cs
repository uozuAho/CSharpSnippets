using System;
using System.IO;

namespace CSharpSnippets.Serialization
{
    public class Packaging
    {
        /// <summary>
        /// Serialize a class to file, then read it back
        /// </summary>
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
                },
            };
            a.MyBRef = a.MyB;

            var packPath = @"C:\temp\blahpack.pack";
            using (var fileStream = new FileStream(packPath, FileMode.Create))
            {
                using (var packer = new ClassDataPacker(fileStream))
                {
                    packer.WriteEntity(a, x => x.Name);
                }
            }

            using (var pack = ClassDataPacker.OpenPack(packPath))
            {
                foreach (var part in pack.ListParts())
                {
                    Console.WriteLine(part.Uri);
                }
                var deserializedA = pack.GetEntity<A>("A_name");
                Console.WriteLine("Deserialized A: " + deserializedA);
            }
        }

        public class A
        {
            public string Name { get; set; }
            public B MyB { get; set; }
            public C MyC { get; set; }
            public B MyBRef { get; set; }
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
