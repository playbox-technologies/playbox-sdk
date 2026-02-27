#if UNITY_EDITOR
using System;
using System.IO;
using Facebook.Unity.Editor;
using Facebook.Unity.Settings;
using Playbox.SdkConfigurations;
using UnityEditor;
using UnityEngine;

namespace Playbox.SdkWindow
{
    public class FacebookSdkWindow : DrawableWindow
    {
        private string appLabel = "";
        private string appId = "";
        private string clientToken = "";
    
        private string prev_appLabel = "";
        private string prev_appId = "";
        private string prev_clientToken = "";
        
        public override void InitName()
        {
            base.InitName();
            
            Name = FacebookSdkConfiguration.Name;
        }
        
        public override void Body()
        {
            if (!Active)
                return;
            
            prev_appLabel = appLabel;
            prev_appId = appId;
            prev_clientToken = clientToken;
            
            GUILayout.BeginHorizontal();
        
            GUILayout.Label("app Label : ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            appLabel = GUILayout.TextField(appLabel, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
        
            GUILayout.EndHorizontal();
            
            EditorGUILayout.Separator();
            
            GUILayout.BeginHorizontal();
        
            GUILayout.Label("client Token : ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            clientToken = GUILayout.TextField(clientToken, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            
            GUILayout.EndHorizontal();
        
            EditorGUILayout.Separator();
            
            GUILayout.BeginHorizontal();
        
            GUILayout.Label("app id : ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            appId = GUILayout.TextField(appId, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
        
            GUILayout.EndHorizontal();
        
            HasUnsavedChanges = !(string.Equals(prev_appLabel, appLabel, StringComparison.OrdinalIgnoreCase) && 
                                  string.Equals(prev_appId, appId, StringComparison.OrdinalIgnoreCase) && 
                                  string.Equals(prev_clientToken, clientToken, StringComparison.OrdinalIgnoreCase));
        
        }

        public override void Save()
        {
            SaveToFacebookSettings();
            
            FacebookSdkConfiguration.Active = Active;
            FacebookSdkConfiguration.AppLabel = appLabel;
            FacebookSdkConfiguration.AppID = appId;
            FacebookSdkConfiguration.ClientToken = clientToken;
            
            FacebookSdkConfiguration.SaveJsonConfig();
            
            base.Save();
        }

        public override void Load()
        {
            FacebookSdkConfiguration.LoadJsonConfig();
         
            Active = FacebookSdkConfiguration.Active;
            appLabel = FacebookSdkConfiguration.AppLabel;
            appId = FacebookSdkConfiguration.AppID;
            clientToken = FacebookSdkConfiguration.ClientToken;
            
            base.Load();
        }

        private void SaveToFacebookSettings()
        {
            var labels = FacebookSettings.AppLabels;
            var appIds = FacebookSettings.AppIds;
            var clientTokens = FacebookSettings.ClientTokens;

            if (labels.Count > 0)
            {
                labels[0] = appLabel;
            }
            else
            {
                labels.Add(appLabel);
            }
                
            if (appIds.Count > 0)
            {
                appIds[0] = appId;
            }
            else
            {
                appIds.Add(appId);
            }
                
            if (clientTokens.Count > 0)
            {
                clientTokens[0] = clientToken;
            }
            else
            {
                clientTokens.Add(clientToken);
            }
        }
    }
}
#endif