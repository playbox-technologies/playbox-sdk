#if UNITY_EDITOR
using System;
using System.IO;
using Editor.Utils.Layout;
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

        public override void HasRenderToggle()
        {
            
        }

        public override void Body()
        {
            PGUI.SpaceLine();
            
            PGUI.Foldout(ref Active, FacebookSdkConfiguration.AppLabel, () =>
            {
                PGUI.HorizontalTextField(ref appLabel, "App Label : ");
                PGUI.Separator();
                PGUI.HorizontalTextField(ref clientToken, "client Token : ");
                PGUI.Separator();
                PGUI.HorizontalTextField(ref appId, "app id : ");
            });
            
            PGUI.SpaceLine();
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