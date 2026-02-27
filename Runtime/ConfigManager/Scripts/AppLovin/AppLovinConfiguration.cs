using System;
using ConfigManager.Scripts.ConfigManagers;
using Newtonsoft.Json.Linq;

namespace ConfigManager.Scripts.AppLovin
{
    public static class AppLovinConfiguration
    {
        public static AppLovinData AppLovinData;
        
        public const string Name = "AppLovin";
        
        public static JObject GetJsonConfig()
        {
            if (AppLovinData == null)
            {
                AppLovinData = new AppLovinData();  
                
            }
            
            return AppLovinData.GetJsonConfig();
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
                AppLovinData = new AppLovinData();    
            }
            else
            {
                AppLovinData = obj.ToObject<AppLovinData>();
            }
        }
    }
}