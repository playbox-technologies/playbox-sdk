using System.IO;
using System.Text;
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
            
            var сChars = DataSerializer.Serialize(json);

            var bytes = Encoding.UTF8.GetBytes(сChars);
            
            File.WriteAllBytes(filePath + ".json.bytes", bytes);
        }

        public static string Load(string configPath)
        {
            var asset = Resources.Load<TextAsset>(configPath);

            var bytes = asset.bytes;
            
            var cChars = Encoding.UTF8.GetChars(bytes);
            
            return DataSerializer.Deserialize(cChars);
        }
    }
}