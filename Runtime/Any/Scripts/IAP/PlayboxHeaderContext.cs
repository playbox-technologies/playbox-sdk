using System.Collections.Generic;

namespace Playbox
{
    public class PlayboxHeaderContext
    {
        public static Dictionary<string, string> GetContext()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            
            result["Content-Type"] = "application/json";
            result["Authorization"] = $"Bearer {Data.Playbox.PlayboxKey}";
            result["X-User-ID"] = Data.Playbox.PlayboxKey;
            result["X-Bundle-ID"] = Data.Playbox.GameId;
            result["X-App-Version"] = Data.Playbox.AppVersion;
            result["X-Platform"] = Data.Playbox.Platform;
            
            return result;
        }
    }
}