#if UNITY_EDITOR

using Playbox.SdkConfigurations;
using UnityEditor;
using UnityEngine;

namespace Playbox.SdkWindow
{
    public class InAppPurchaseWindow : DrawableWindow
    {
        bool useCustom = false;

        private string uri;
        private string token;
        
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
            uri = EditorGUILayout.TextField("", uri, GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            
            GUILayout.EndHorizontal();
            
            EditorGUILayout.Separator();
            
            GUILayout.BeginHorizontal();
            
            GUILayout.Label("Token: ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            token = EditorGUILayout.TextField("", token, GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            
            GUILayout.EndHorizontal();
        }

        public override void Save()
        {
            InAppConfiguration.UseCustomInApp = useCustom;
            InAppConfiguration.Active = active;
            
            InAppConfiguration.SaveJsonConfig();
        }

        public override void Load()
        {
            InAppConfiguration.LoadJsonConfig();
            
            useCustom = InAppConfiguration.UseCustomInApp;
            active = InAppConfiguration.Active;
        }
    }
}
#endif