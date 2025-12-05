using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.IO;

namespace LD_PaymentEngine.APIServices
{
    public class JsonUtils
    {
        public static T GetObjectFromJsonString<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static dynamic GetDynamicFromJsonString(string json)
        {
            return JsonConvert.DeserializeObject<dynamic>(json);
        }
        public static T GetObjectFromJsonFile<T>(string jsonPath)
        {
            return JsonConvert.DeserializeObject<T>(GetJsonStringFromFile(jsonPath));
        }
        public static string GetJsonStringFromFile(string jsonPath)
        {
            if (!jsonPath.Contains(".json"))
                jsonPath = jsonPath + ".json";

            return File.ReadAllText(jsonPath);
        }

        //public static string GetJsonStringFromFile(string jsonPath, string method)
        //{
        //    if (!jsonPath.Contains(".json"))
        //        jsonPath = jsonPath + ".json";

        //    var json = File.ReadAllText(jsonPath);

        //    var objects = JArray.Parse(json); // parse as array  
        //    foreach (JObject root in objects)
        //    {
        //        foreach (KeyValuePair<String, JToken> app in root)
        //        {
        //            var appName = app.Key;
                    


        //        }
        //    }
        //}
        public static string GetJsonStringFromObject(object jsonobject)
        {
            return JsonConvert.SerializeObject(jsonobject);
        }

    }
}