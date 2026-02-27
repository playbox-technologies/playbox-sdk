using System;
using ConfigManager.Scripts.ConfigManagers;
using Newtonsoft.Json.Linq;

namespace ConfigManager.Scripts.DevToDev
{
    public static class DevToDevConfiguration{
        
        public static DevToDevData DevToDevData;
        
        public const string Name = "DevToDev";

        public static bool Active
        {
            get
            {
                if (DevToDevData == null)
                    return false;
                
                return DevToDevData.active;
            }
        }

        private static JObject GetJsonConfig()
        {
            if (DevToDevData == null)
            {
                DevToDevData = new DevToDevData();
            }
        
            return DevToDevData.GetJsonConfig();
        }

        public static void SaveJsonConfig()
        {
            GlobalPlayboxConfig.SaveSubconfigs(Name ,GetJsonConfig());
        }
    
        public static void LoadJsonConfig()
        {
            JObject obj = GlobalPlayboxConfig.LoadSubconfigs(Name);

            if (obj == null)
            {
                DevToDevData = new DevToDevData();
            }
            else
            {
                DevToDevData = obj.ToObject<DevToDevData>();
            }
        }

    }
}