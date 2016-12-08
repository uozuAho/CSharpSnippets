using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        public static void JsonNetDeserializeWithoutType()
        {
            var testObj = TestObj.GetDefault();
            var path = @"C:\temp\TestObj.json";
            // wrap with type name so we know what type to deserialize to
            var outputObj = new Dictionary<string, object> { { "CSharpSnippets.Serialization.TestObj", testObj } };
            File.WriteAllText(path, JsonConvert.SerializeObject(outputObj, Formatting.Indented));

            var json = File.ReadAllText(path);
            // the top level type must be known, otherwise we can't traverse through it
            var inputObj = (Dictionary<string, JObject>)JsonConvert.DeserializeObject(json, typeof(Dictionary<string, JObject>));
            foreach (var kv in inputObj)
            {
                var typename = kv.Key;
                var type = Type.GetType(typename);
                var typedObj = kv.Value.ToObject(type);
                Console.WriteLine(typedObj);
            }
        }
    }
}
