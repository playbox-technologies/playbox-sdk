#if UNITY_EDITOR

using System.IO;
using Newtonsoft.Json.Linq;
using Playbox.SdkConfigurations;
using UnityEditor;
using UnityEngine;
using Utils.Tools.Extentions;

namespace Playbox
{
    public static class PlayboxMultiSceneInstaller
    {
        private const string flagKey = "playbox_helper";
        
        [InitializeOnEnterPlayMode]
        static void InstallPlaybox()
        {
            bool enabled = EditorPrefs.GetBool(flagKey, true);
            
            if(!enabled)
                return;
            
            EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    var mainInit = Object.FindFirstObjectByType<MainInitialization>();

                    if (mainInit == null)
                    {
                        var go = new GameObject("Playbox SDK Installers");
              
                        go.AddComponent<MainInitialization>();  
              
                    }
                }
            };
        }

        [MenuItem("Playbox/Utils/Auto Initialize Playbox")]
        public static void ToggleHelper()
        {
            bool enabled = !EditorPrefs.GetBool(flagKey, true);
            
            
            if(enabled)
                "Enable Editor Helper".PbLog("HELPER");
            else
                "Disable Editor Helper".PbLog("HELPER");
            
            EditorPrefs.SetBool(flagKey, enabled);
        }

        [MenuItem("Playbox/Utils/Auto Initialize Playbox", true)]
        public static bool ToggleHelperValidate()
        {
            bool enabled = EditorPrefs.GetBool(flagKey, true);
            
            Menu.SetChecked("Playbox/Utils/Auto Initialize Playbox", enabled);
            
            return true;
        }
        
    }  
}

#endif