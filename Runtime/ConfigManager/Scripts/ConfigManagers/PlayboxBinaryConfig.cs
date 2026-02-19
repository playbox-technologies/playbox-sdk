using System.IO;
using System.Text;
using UnityEngine;
using Utils.Tools.Serializer;

namespace ConfigManager.Scripts.ConfigManagers
{
    public static class PlayboxBinaryConfig 
    {
        public static void Save(string configPath, string json)
        {
            string directory = Path.GetDirectoryName(configPath);
            
            if (!Directory.Exists(directory))
            {
                if (directory != null) 
                    Directory.CreateDirectory(directory);
            }
            
            var сChars = DataSerializer.Serialize(json);

            var bytes = Encoding.UTF8.GetBytes(сChars);
            
            File.WriteAllBytes(configPath, bytes);
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