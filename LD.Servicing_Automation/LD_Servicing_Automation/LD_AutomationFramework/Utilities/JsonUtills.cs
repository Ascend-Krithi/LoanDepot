using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.Json;

namespace LD_AutomationFramework.Utilities
{
    public class JsonUtils
    {
        /// <summary>
        /// Deserialize Json string to Object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="json">json string</param>
        /// <returns></returns>
        public static T GetObjectFromJsonString<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Deserialize Json string to dynamic Object
        /// </summary>
        /// <param name="json">json string</param>
        /// <returns></returns>
        public static dynamic GetDynamicFromJsonString(string json)
        {
            return JsonConvert.DeserializeObject<dynamic>(json);
        }

        /// <summary>
        /// Deserialize Json string from file to Object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="jsonPath">json file path</param>
        /// <returns></returns>

        public static T GetObjectFromJsonFile<T>(string jsonPath)
        {
            return JsonConvert.DeserializeObject<T>(GetJsonStringFromFile(jsonPath));
        }
        /// <summary>
        /// string formmated json from file
        /// </summary>
        /// <param name="jsonPath">path to .json file</param>
        /// <returns></returns>
        public static string GetJsonStringFromFile(string jsonPath)
        {
            if (!jsonPath.Contains(".json"))
                jsonPath = jsonPath + ".json";

            return File.ReadAllText(jsonPath);
        }

        /// <summary>
        /// Serialize json object to string format
        /// </summary>
        /// <param name="jsonobject">json object</param>
        /// <returns></returns>
        public static string GetJsonStringFromObject(object jsonobject)
        {
            return JsonConvert.SerializeObject(jsonobject);
        }

        /// <summary>
        /// Beautify/Format the json string in printable format
        /// </summary>
        /// <param name="jsonobject">json object</param>
        /// <returns></returns>
        public static string FormatJson(object jsonobject)
        {
            JsonDocument doc = JsonDocument.Parse(jsonobject.ToString());
            return System.Text.Json.JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });

        }

    }
}