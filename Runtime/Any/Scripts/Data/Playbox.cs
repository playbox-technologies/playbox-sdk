using UnityEngine;

namespace Playbox.Data
{
    public static class Playbox
    {
        public static string AppVersion => Application.version;
        public static string GameId => Application.identifier;
        public static string Campaign => Application.companyName;
        
        public const string PlayboxKey = "plx_api_Rm8qTXe7Pzw94v1FujgEKsWD";

        public static void SetVersion(string version)
        {
            
        }
    }
}