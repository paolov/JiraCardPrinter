using Newtonsoft.Json;
using System.IO;

namespace PrintJiraCards.Controllers
{
    public static class SerializationExtensions
    {
        public static void SaveTo<T>(this T value, string filename)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            File.WriteAllText(filename, JsonConvert.SerializeObject(value, settings));
        }

        public static T ReadFrom<T>(this string filename)
        {
            var jsonData = File.ReadAllText(filename);

            if (string.IsNullOrEmpty(jsonData)) return default(T);

            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}