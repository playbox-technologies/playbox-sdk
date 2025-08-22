using System.IO;
using UnityEngine;
using Utils.Data;

namespace Playbox.SdkConfigurations
{
    public static class PlayboxBinaryConfig 
    {
        public static void Save(string configPath,string filePath, string json)
        {
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
            
            var bytes = DataSerializer.Serialize(json);
        
            File.WriteAllBytes(filePath + ".bytes", bytes);
        }

        public static string Load(string configPath)
        {
            var asset = Resources.Load<TextAsset>("Playbox/PlayboxConfig/playbox_sdk_config.json");
            
            return DataSerializer.Deserialize<string>(asset.bytes);
                
        }
    }
}