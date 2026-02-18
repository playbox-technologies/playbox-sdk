using Newtonsoft.Json.Linq;

namespace Utils.Tools.Extentions
{
    public static class JsonPlayboxExtentions
    {
        public static bool GetBoolConfig(this JObject obj, string key)
        {
            if (obj.TryGetValue(key, out var value))
            {
                if (value.Type == JTokenType.Boolean)
                {
                    return value.Value<bool>();  
                }
                
                return false;
            }
            
            return false;
        }

        public static string GetStringConfig(this JObject obj, string key)
        {
            if (obj.TryGetValue(key, out var value))
            {
                if (value.Type == JTokenType.String)
                {
                    return value.Value<string>();  
                }

                return "";

            }
            return "";
        }
    }
}