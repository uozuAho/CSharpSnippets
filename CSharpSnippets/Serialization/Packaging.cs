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
            var testObj = TestObj.GetDefault();

            var packPath = @"C:\temp\blahpack.pack";
            using (var fileStream = new FileStream(packPath, FileMode.Create))
            {
                using (var packer = new ClassDataPacker(fileStream))
                {
                    packer.WriteEntity(testObj, x => x.Name);
                }
            }

            using (var pack = ClassDataPacker.OpenPack(packPath))
            {
                foreach (var part in pack.ListParts())
                {
                    Console.WriteLine(part.Uri);
                }
                var deserializedObj = pack.GetEntity<TestObj>("A_name");
                Console.WriteLine("Deserialized TestObj: " + deserializedObj);
            }
        }
    }
}
