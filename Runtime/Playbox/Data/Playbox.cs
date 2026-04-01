using System;
using UnityEngine;

namespace Playbox.Data
{
    public static class Playbox
    {
        public static string AppVersion => Application.version;
        public static string GameId => Application.identifier;
        public static string Campaign => Application.companyName;
        public static string Platform => GetPlatform();
        
        public const string PlayboxKey = "pbx_live_5289164063088009792ba4d94a1de74592e0c1f567ae515b";
        

        public static void SetVersion(string version)
        {
            
        }

        private static string GetPlatform() => Application.platform switch
        {
            RuntimePlatform.IPhonePlayer => "ios",
            RuntimePlatform.Android => "android",
            _ => throw new ArgumentOutOfRangeException()
        };

    }
}