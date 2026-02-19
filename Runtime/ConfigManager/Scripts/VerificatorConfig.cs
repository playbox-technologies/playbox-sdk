using ConfigManager.Scripts.ConfigManagers;
using Newtonsoft.Json.Linq;
using Utils.Tools.Extentions;

namespace Playbox.SdkConfigurations
{
    public class VerificatorConfig
    {
        private static string _serverURL;
        private static string _serverKey;
    
        private static bool active = false;

        private const string name = "IAP Verificator";
        

        public static bool Active
        {
            get => active;
            set => active = value;
        }

        public static string Name
        {
            get => name;
        }

        public static string ServerURL
        {
            get => _serverURL;
            set => _serverURL = value;
        }
        
        public static string ServerKey
        {
            get => _serverKey;
            set => _serverKey = value;
        }


        public static JObject GetJsonConfig()
        {
            JObject config = new JObject();
        
            config[nameof(ServerURL)] = ServerURL;
            config[nameof(ServerKey)] = ServerKey;
            config[nameof(active)] = active;
        
            return config;
        }

        public static void SaveJsonConfig()
        {
            GlobalPlayboxConfig.SaveSubconfigs(Name,GetJsonConfig());
        }
    
        public static void LoadJsonConfig()
        {
            JObject obj = GlobalPlayboxConfig.LoadSubconfigs(Name);

            if (obj == null)
            {
                $"{Name} config not contains in json".PbLog();
            
                return;
            }
        
            ServerURL = (string)(obj[nameof(ServerURL)] ?? false);
            ServerKey = (string)(obj[nameof(ServerKey)] ?? false);
            active = (bool)(obj[nameof(active)] ?? false);
            
        }
    }
}