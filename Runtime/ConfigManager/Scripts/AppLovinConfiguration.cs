using Newtonsoft.Json.Linq;
using Utils.Tools.Extentions;

namespace Playbox.SdkConfigurations
{ 
    /// <summary>
    /// Provides configuration management for AppLovin SDK integration with Playbox, including saving and loading JSON configurations.
    /// </summary>
    public static class AppLovinConfiguration
    {
        private static bool _isUseReward = true;
        private static bool _isUseInterstitial = true;
        private static string _iosKeyRew = "";
        private static string _iosKeyInter = "";
        private static string _androidKeyRew = "";
        private static string _androidKeyInter = "";
        private static string advertisementSdk = "";
    
        private static bool active = false;

        public const string name = "AppLovin";

        public static string IOSKey_rew
        {
            get => _iosKeyRew;
            set => _iosKeyRew = value;
        }
        
        public static string IOSKey_inter
        {
            get => _iosKeyInter;
            set => _iosKeyInter = value;
        }

        public static string AndroidKey_rew
        {
            get => _androidKeyRew;
            set => _androidKeyRew = value;
        } 
        
        public static string AndroidKey_iter
        {
            get => _androidKeyInter;
            set => _androidKeyInter = value;
        }

        public static bool Active
        {
            get => active;
            set => active = value;
        }

        public static string AdvertisementSdk
        {
            get => advertisementSdk;
            set => advertisementSdk = value;
        }
        
        public static bool IsUseReward
        {
            get => _isUseReward;
            set => _isUseReward = value;
        }

        public static bool IsUseInterstitial
        {
            get => _isUseInterstitial;
            set => _isUseInterstitial = value;
        }


        public static JObject GetJsonConfig()
        {
            JObject config = new JObject();
        
            config[nameof(_iosKeyRew)] = _iosKeyRew;
            config[nameof(_iosKeyInter)] = _iosKeyInter;
            config[nameof(_androidKeyRew)] = _androidKeyRew;
            config[nameof(_androidKeyInter)] = _androidKeyInter;
            config[nameof(advertisementSdk)] = AdvertisementSdk;
            config[nameof(active)] = active;
            config[nameof(_isUseInterstitial)] = _isUseInterstitial;
            config[nameof(_isUseReward)] = _isUseReward;
        
            return config;
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
                $"{name} config not contains in json".PlayboxWarning();
            
                return;
            }
        
            _iosKeyRew = (string)obj[nameof(_iosKeyRew)];
            _iosKeyInter = (string)obj[nameof(_iosKeyInter)];
            _androidKeyRew = (string)obj[nameof(_androidKeyRew)];
            _androidKeyInter = (string)obj[nameof(_androidKeyInter)];
            advertisementSdk = (string)obj[nameof(advertisementSdk)];
            active = (bool)(obj[nameof(active)] ?? false);
            _isUseInterstitial = (bool)(obj[nameof(_isUseInterstitial)] ?? false);
            _isUseReward = (bool)(obj[nameof(_isUseReward)] ?? false);
        
        }

    }
}