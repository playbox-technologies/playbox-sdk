using System.Globalization;
using System.IO;
using CI.Utils.Extentions;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Playbox.SdkConfigurations
{
    /// <summary>
    /// Manages the global configuration for the Playbox SDK, including loading, saving, and handling subconfigurations.
    /// </summary>
    public static class GlobalPlayboxConfig 
    {
        public static string configPath => Path.Combine(Application.dataPath, "Resources", "Playbox", "PlayboxConfig");
        
        public static bool IsLoaded => jsonConfig != null;
        
        private static JObject jsonConfig = new();
        private static string configFile = Path.Combine(Application.dataPath, "Resources", "Playbox", "PlayboxConfig",
            "playbox_sdk_config.json");
        
#if UNITY_EDITOR
        
        public static void Save()
        {
            PlayboxBinaryConfig.Save(configPath,configFile,jsonConfig.ToString());
            
            UnityEditor.AssetDatabase.Refresh();
        }
        
        public static void SaveSubconfigs(string name, JObject config)
        {
            if(jsonConfig == null)
                jsonConfig = new JObject();
            
            jsonConfig[name] = config;
        }
        
        public static void Clear()
        {
            jsonConfig = new JObject();
        }
        
#endif
        
        public static void Load()
        {
             if (File.Exists(configFile + ".bin"))
             {
                 jsonConfig = JObject.Parse(PlayboxBinaryConfig.Load(configFile));
             }
             else
             {
                 jsonConfig = JObject.Parse(PlayboxJsonConfig.Load(configFile));
             }
        }

        public static JObject LoadSubconfigs(string name)
        {
            if (jsonConfig[name] == null)
                return null;
            
            return jsonConfig[name].ToObject<JObject>();
        }
    }
}