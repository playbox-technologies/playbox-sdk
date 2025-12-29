using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
            "playbox_sdk_config");
        
        private static string ResourcesPath = "Playbox/PlayboxConfig/playbox_sdk_config";
        
        public static void Save()
        {
            PlayboxBinaryConfig.Save(configPath,configFile,jsonConfig.ToString());
            
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
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
        
        public static void Load()
        {
            if (Resources.Load<TextAsset>(ResourcesPath + ".json"))
            {
                jsonConfig = JObject.Parse(PlayboxBinaryConfig.Load(ResourcesPath + ".json"));
            }
            else
            {
                jsonConfig = JObject.Parse(PlayboxJsonConfig.Load(ResourcesPath));   
            }
        }

        public static JObject LoadSubconfigs(string name)
        {
            return jsonConfig[name]?.ToObject<JObject>();
        }
    }
}