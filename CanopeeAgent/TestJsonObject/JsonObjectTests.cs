using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Canopee.Common;
using Newtonsoft.Json;
using NUnit.Framework;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace TestJsonObject
{
    public class JsonObjectTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Should_Get_JsonObject_From_File_With_NoChild_Properties_Standard_Types()
        {
            var jsonObject = JsonObject.LoadFromFile("simple.json");
            Assert.AreEqual("aStringValue", jsonObject.GetProperty<String>("PropertyString"));
            Assert.IsTrue(jsonObject.GetProperty<bool>("PropertyBool"));
            Assert.AreEqual(1.0f, jsonObject.GetProperty<float>("PropertyNumeric"));
            Assert.AreEqual(1, jsonObject.GetProperty<short>("PropertyInt"));
            Assert.IsNull(jsonObject.GetProperty<String>("PropertyNull"));
        }
        
        [Test]
        public void Should_Get_JsonObject_From_File_With_Array_Same_Type()
        {
            var jsonObject = JsonObject.LoadFromFile("witharray.json");
            Assert.That(jsonObject.GetProperty<List<object>>("PropertyArray"), Is.EquivalentTo(new List<object>() { "a","b"}));
        }
        
        [Test]
        public void Should_Get_JsonObject_From_File_With_Array_Multiple_Type()
        {
            var jsonObject = JsonObject.LoadFromFile("witharray.json");
            Assert.That(jsonObject.GetProperty<List<object>>("PropertyVariousArray"), Is.EquivalentTo(new List<object>() { "a","b",1,1.0f}));
        }

        [Test]
        public void Should_Get_JsonObject_From_File_With_Object_Properties()
        {
            var jsonObject = JsonObject.LoadFromFile("withobject.json").GetProperty<JsonObject>("PropertyObject");
            Assert.AreEqual("anotherStringValue", jsonObject.GetProperty<String>("PropertyString"));
            Assert.IsFalse(jsonObject.GetProperty<bool>("PropertyBool"));
            Assert.AreEqual(2.59f, jsonObject.GetProperty<float>("PropertyNumeric"));
            Assert.AreEqual(2, jsonObject.GetProperty<short>("PropertyInt"));
            Assert.IsNull(jsonObject.GetProperty<String>("PropertyNull"));
            Assert.That(jsonObject.GetProperty<List<object>>("PropertyVariousArray"), Is.EquivalentTo(new List<object>() { "c","d",2,2.78f}));
            Assert.That(jsonObject.GetProperty<List<object>>("PropertyArray"), Is.EquivalentTo(new List<object>() { "c","d"}));
        }

        [Test]
        public void Should_serialize_same_things_after_loading_object()
        {
            var expected = File.ReadAllText("withobject.json");
            var jsonObject = JsonObject.LoadFromFile("withobject.json");
            Assert.AreEqual(expected, jsonObject.ToString());
        }

        [Test]
        public void Should_set_a_new_value_property_in_a_loaded_object()
        {
            var expected = File.ReadAllText("withobject.json");
            var jsonObject = JsonObject.LoadFromFile("withobject.json");
            jsonObject.SetProperty("NewPropertyNumeric", 1);
            jsonObject.SetProperty("NewPropertyString", "aNewProperty");
            Assert.AreEqual(1,jsonObject.GetProperty<int>("NewPropertyNumeric"));
            Assert.AreEqual("aNewProperty",jsonObject.GetProperty<string>("NewPropertyString"));
        }

        [Test]
        public void Should_write_a_file_with_serialized_json()
        {
            var tempFile = Path.GetTempFileName();
            var jsonObject = JsonObject.LoadFromFile("withobject.json");
            jsonObject.WriteTo(tempFile);
            Assert.IsTrue(File.Exists(tempFile));
            var fileContent = File.ReadAllText(tempFile);
            Assert.AreEqual(jsonObject.ToString(),fileContent);
        }
    }
}