using System.IO;
using UnityEngine;

namespace Playbox.SdkConfigurations
{
    public static class PlayboxJsonConfig
    {
        public static void Save(string configPath,string filePath, string json)
        {
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
        
            File.WriteAllText(filePath, json);
        }

        public static string Load(string configPath)
        {
            var asset = Resources.Load<TextAsset>(configPath);

            if (asset == null)
                return "{}";
            
            return asset.text;
        }
    }
}