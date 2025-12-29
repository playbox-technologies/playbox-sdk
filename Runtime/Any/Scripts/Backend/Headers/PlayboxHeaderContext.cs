using System.Collections.Generic;

namespace Any.Scripts.Backend
{
    public static class PlayboxHeaderContext
    {
        public static Dictionary<string, string> GetContext()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            
            result["Content-Type"] = "application/json";
            result["Authorization"] = $"Bearer {Playbox.Data.Playbox.PlayboxKey}";
            result["X-User-ID"] = Playbox.Data.Playbox.PlayboxKey;
            result["X-Bundle-ID"] = Playbox.Data.Playbox.GameId;
            result["X-App-Version"] = Playbox.Data.Playbox.AppVersion;
            result["X-Platform"] = Playbox.Data.Playbox.Platform;
            
            return result;
        }
    }
}