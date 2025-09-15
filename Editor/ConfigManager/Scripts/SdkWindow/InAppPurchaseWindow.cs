#if UNITY_EDITOR

using Playbox.SdkConfigurations;
using UnityEditor;
using UnityEngine;

namespace Playbox.SdkWindow
{
    public class InAppPurchaseWindow : DrawableWindow
    {
        bool useCustom = false;

        private string url;
        private string serverKey;
        
        public override void InitName()
        {
            base.InitName();

            name = InAppConfiguration.Name;
        }

        public override void Body()
        {
            if (!active)
                return;
            
            GUILayout.Label("IAP Verification Module",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            
            GUILayout.BeginHorizontal();
            
            GUILayout.Label("URL: ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            url = EditorGUILayout.TextField("", url, GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            
            GUILayout.EndHorizontal();
            
            EditorGUILayout.Separator();
            
            GUILayout.BeginHorizontal();
            
            GUILayout.Label("Token: ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            serverKey = EditorGUILayout.TextField("", serverKey, GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            
            GUILayout.EndHorizontal();
        }

        public override void Save()
        {
            InAppConfiguration.UseCustomInApp = useCustom;
            InAppConfiguration.Active = active;
            
            VerificatorConfig.Active = active;
            VerificatorConfig.ServerURL = url;
            VerificatorConfig.ServerKey = serverKey;
            
            VerificatorConfig.SaveJsonConfig();
            InAppConfiguration.SaveJsonConfig();
        }

        public override void Load()
        {
            VerificatorConfig.LoadJsonConfig();
            InAppConfiguration.LoadJsonConfig();
            
            
            useCustom = InAppConfiguration.UseCustomInApp;
            active = InAppConfiguration.Active;
            
            active = VerificatorConfig.Active;
            url = VerificatorConfig.ServerURL;
            serverKey = VerificatorConfig.ServerKey;
        }
    }
}
#endif