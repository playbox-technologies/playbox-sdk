using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConfigManager.Scripts.AppLovin
{
    [Serializable]
    public class AppLovinData
    {
        [JsonProperty("_isAsync")]
        public bool isAsync;
        
        [JsonProperty("_isConfigured")]
        public bool isConfigured;
        
        [JsonProperty("_isUseReward")]
        public bool isUseReward;
        
        [JsonProperty("_isUseInterstitial")]
        public bool isUseInterstitial;
        
        [JsonProperty("_iosKeyRew")]
        public string iosKeyRew;
        
        [JsonProperty("_iosKeyInter")]
        public string iosKeyInter;
        
        [JsonProperty("_androidKeyRew")]
        public string androidKeyRew;
        
        [JsonProperty("_androidKeyInter")]
        public string androidKeyInter;
        
        [JsonProperty("advertisementSdk")]
        public string advertisementSdk;
        
        [JsonProperty("active")]
        public bool active;
        
        public JObject GetJsonConfig()
        {
            return JObject.FromObject(this);
        }
    }
}