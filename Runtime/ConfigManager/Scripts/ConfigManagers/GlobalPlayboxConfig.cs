using System;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ConfigManager.Scripts.ConfigManagers
{
    /// <summary>
    /// Manages the global configuration for the Playbox SDK, including loading, saving, and handling subconfigurations.
    /// </summary>
    public static class GlobalPlayboxConfig 
    {
        private static JObject _jsonConfig = new();
        
        private static string ConfigPath => Path.Combine(Application.dataPath, "Resources", "Playbox", "PlayboxConfig", DefaultConfigFile + ".bytes");
        
        private static readonly string ResourcesPath = Path.Combine("Playbox","PlayboxConfig", DefaultConfigFile);
        
        private const string DefaultConfigFile = "playbox_sdk_config.json";
        
        public static void Save()
        {
            PlayboxBinaryConfig.Save(ConfigPath,_jsonConfig.ToString());
            
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        
        public static void SaveSubconfigs(string name, JObject config)
        {
            if(_jsonConfig == null)
                _jsonConfig = new JObject();
            
            _jsonConfig[name] = config;
        }
        
        public static void Clear()
        {
            if (_jsonConfig != null)
            {
                _jsonConfig.RemoveAll();
            }
            else
            {
                _jsonConfig = new JObject();   
            }
        }
        
        public static bool Load()
        {
            if (ExistsBinaryJson())
            {
                _jsonConfig = JObject.Parse(PlayboxBinaryConfig.Load(ResourcesPath));   
                return true;
            }
            else
            {
                Debug.LogError("The playbox config file does not exist.");
                return false;
            }
        }
        
        public static bool ExistsBinaryJson()
        {
            return File.Exists(ConfigPath);
        }

        public static JObject LoadSubconfigs(string name)
        {
            if (_jsonConfig != null)
            {
                if (_jsonConfig.TryGetValue(name, out var value))
                {
                    if (value.Type == JTokenType.Object)
                    {
                        return (JObject)value;
                    }   
                }
            }
            else
            {
                throw new Exception("The playbox config file does not exist.");    
            }

            return null;
        }
    }
}