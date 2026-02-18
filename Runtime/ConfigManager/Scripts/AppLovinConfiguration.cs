using System;
using Newtonsoft.Json.Linq;
using Utils.Tools.Extentions;
using Utils.Tools.Serializer;

namespace Playbox.SdkConfigurations
{
    /// <summary>
    /// Provides configuration management for AppLovin SDK integration with Playbox, including saving and loading JSON configurations.
    /// </summary>
    ///

    [System.Serializable]
    public class AppLovinData
    {
        public bool _isAsync;
        public bool _isConfigured;
        public bool _isUseReward;
        public bool _isUseInterstitial;
        public string _iosKeyRew;
        public string _iosKeyInter;
        public string _androidKeyRew;
        public string _androidKeyInter;
        public string advertisementSdk;
        public bool active;
        
        public JObject GetJsonConfig()
        {
            JObject config = new JObject();
        
            config[nameof(_iosKeyRew)] = _iosKeyRew;
            config[nameof(_iosKeyInter)] = _iosKeyInter;
            config[nameof(_androidKeyRew)] = _androidKeyRew;
            config[nameof(_androidKeyInter)] = _androidKeyInter;
            config[nameof(advertisementSdk)] = advertisementSdk;
            config[nameof(active)] = active;
            config[nameof(_isUseInterstitial)] = _isUseInterstitial;
            config[nameof(_isUseReward)] = _isUseReward;
        
            return config;
        }
    }

    public static class AppLovinConfiguration
    {
        public static AppLovinData appLovinData;
        
        public const string name = "AppLovin";
        
        public static JObject GetJsonConfig()
        {
            if (appLovinData == null)
                throw new Exception("AppLovinData is null");
            
            return appLovinData.GetJsonConfig();
        }

        public static void SaveJsonConfig()
        {
            GlobalPlayboxConfig.SaveSubconfigs(name,GetJsonConfig());
        }
    
        public static void LoadJsonConfig()
        {
            JObject obj = GlobalPlayboxConfig.LoadSubconfigs(name);

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