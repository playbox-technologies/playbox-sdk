using System;
using DevToDev.Analytics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConfigManager.Scripts.DevToDev
{
    [Serializable]
    public class DevToDevData
    {
        
        [JsonProperty("ios_key")]
        public string iosKey = "";
        
        [JsonProperty("android_key")]
        public string androidKey = "";
        
        [JsonProperty("logLevel")]
        public int logLevel = 0;
        
        [JsonProperty("active")]
        public bool active;
        
        public JObject GetJsonConfig()
        {
            return JObject.FromObject(this);
        }
    }
}