using Newtonsoft.Json.Linq;

namespace Utils.Tools.Serializer
{
    public static class ConfigHelper
    {
        public static T Get<T>(this JObject dict, string key, T defaultValue = default)
        {
            if (dict.TryGetValue(key, out var value) && value is T castedValue)
                return castedValue;
        
            return defaultValue;
        }
    }
}