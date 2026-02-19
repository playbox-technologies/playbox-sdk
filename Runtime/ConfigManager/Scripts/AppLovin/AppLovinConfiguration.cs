using System;
using Newtonsoft.Json.Linq;
using Playbox.SdkConfigurations;

namespace ConfigManager.Scripts.AppLovin
{
    public static class AppLovinConfiguration
    {
        public static AppLovinData appLovinData;
        
        public const string Name = "AppLovin";
        
        public static JObject GetJsonConfig()
        {
            if (appLovinData == null)
                throw new Exception("AppLovinData is null");
            
            return appLovinData.GetJsonConfig();
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
                appLovinData = new AppLovinData();    
            }
            else
            {
                appLovinData = obj.ToObject<AppLovinData>();
            }
        }
    }
}