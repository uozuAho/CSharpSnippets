using Newtonsoft.Json;
using System;
using System.IO;

namespace CSharpSnippets.Serialization
{
    class JsonNet
    {
        public static void JsonNetExample()
        {
            var testObj = TestObj.GetDefault();
            var path = @"C:\temp\TestObj.json";
            File.WriteAllText(path, JsonConvert.SerializeObject(testObj, Formatting.Indented));

            var deserializedObj = JsonConvert.DeserializeObject<TestObj>(File.ReadAllText(path));
            Console.WriteLine("Deserialized TestObj: " + deserializedObj);
        }
    }
}
