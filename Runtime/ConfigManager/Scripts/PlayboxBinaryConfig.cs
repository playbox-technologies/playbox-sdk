using System.IO;
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
        
            File.WriteAllBytes(filePath + ".bin", bytes);
        }

        public static string Load(string configPath)
        {
            var path = configPath + ".bin";
            
            if (File.Exists(path))
            {
                var bytes =  File.ReadAllBytes(path);
                
                return DataSerializer.Deserialize<string>(bytes);
            }
            
            return "";
        }
    }
}