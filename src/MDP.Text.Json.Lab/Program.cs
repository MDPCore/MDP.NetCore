using System;
using System.Diagnostics;
using System.Text.Json;

namespace MDP.Text.Json.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // SerializerOptions
            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new TimeSpanJsonConverter());

            // Serialize 
            var serializeObject = new TestClass { Duration = new TimeSpan(2, 14, 18) };
            Console.WriteLine($"Serialized:   {serializeObject.Duration}");

            // JSON String
            var serializeObjectString = JsonSerializer.Serialize(serializeObject, serializerOptions);
            Console.WriteLine($"JSON String:  {serializeObjectString}");

            // Deserialize
            var deserializedObject = JsonSerializer.Deserialize<TestClass>(serializeObjectString, serializerOptions);
            Console.WriteLine($"Deserialized: {deserializedObject.Duration}");
        }
    }

    public class TestClass
    {
        // Properties
        public TimeSpan Duration { get; set; }
    }
}
