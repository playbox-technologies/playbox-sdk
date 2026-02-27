using System;
using ConfigManager.Scripts.ConfigManagers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utils.Tools.Extentions;

namespace Playbox.SdkConfigurations
{

    [Serializable]
    public class FacebookSDKData
    {
        [JsonProperty("appLabel")]
        public string appLabel = "";
        [JsonProperty("app_id")]
        public string appID = "";
        [JsonProperty("clientToken")]
        public string clientToken = "";
        [JsonProperty("active")]
        public bool active;
        
        public JObject GetJsonConfig()
        {
            return JObject.FromObject(this);
        }
    }
    
    public static class FacebookSdkConfiguration
    {

        public static FacebookSDKData FacebookSDKData;

        public const string Name = "FacebookSdk";
        
        public static bool Active
        {
            get
            {
                if (FacebookSDKData == null)
                    return false;
                
                return FacebookSDKData.active;
            }
        }
        
        public static JObject GetJsonConfig()
        {
            if (FacebookSDKData == null)
            {
                FacebookSDKData = new FacebookSDKData();
            }
            
            return FacebookSDKData.GetJsonConfig();
        }

        public static void SaveJsonConfig()
        {
            GlobalPlayboxConfig.SaveSubconfigs(Name,GetJsonConfig());
        }
    
        public static void LoadJsonConfig()
        {
            JObject obj = GlobalPlayboxConfig.LoadSubconfigs(Name);

            if (obj == null)
            {
                $"{Name} config not contains in json".PbLog();
                
                FacebookSDKData = new FacebookSDKData();
            }
            else
            {
                FacebookSDKData = obj.ToObject<FacebookSDKData>();
            }
        
           
            
        }

    }
}