using System.IO;

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
            string jsonText = "";
            
            if (File.Exists(configPath))
            {
                jsonText =  File.ReadAllText(configPath);
            }
            
            return jsonText;
        }
    }
}