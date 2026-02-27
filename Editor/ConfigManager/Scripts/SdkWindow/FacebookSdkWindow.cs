#if UNITY_EDITOR
using Editor.Utils.Layout;
using Facebook.Unity.Settings;
using Playbox.SdkConfigurations;
using Playbox.SdkWindow;
using UnityEngine;

namespace Editor.ConfigManager.Scripts.SdkWindow
{
    public class FacebookSdkWindow : DrawableWindow
    {
        private FacebookSDKData _facebookSDKData;
        public override void InitName()
        {
            base.InitName();
            
            Name = FacebookSdkConfiguration.Name;
            
            _facebookSDKData = new FacebookSDKData();
        }

        public override void HasRenderToggle()
        {
            
        }

        public override void Body()
        {
            if (_facebookSDKData == null)
            {
                Debug.LogError("facebookSDKData is null");
                return;
            }
            
            PGUI.SpaceLine();
            
            PGUI.Foldout(ref _facebookSDKData.active, FacebookSdkConfiguration.Name, () =>
            {
                PGUI.HorizontalTextField(ref _facebookSDKData.appLabel, "App Label : ");
                PGUI.Separator();
                PGUI.HorizontalTextField(ref _facebookSDKData.appID, "app id : ");
                PGUI.Separator();
                PGUI.HorizontalTextField(ref _facebookSDKData.clientToken, "client Token : ");
            });
            
            PGUI.SpaceLine();
        }

        public override void Save()
        {
            SaveToFacebookSettings();
            
            FacebookSdkConfiguration.FacebookSDKData = _facebookSDKData;
            
            FacebookSdkConfiguration.SaveJsonConfig();
            
            base.Save();
        }

        public override void Load()
        {
            FacebookSdkConfiguration.LoadJsonConfig();
         
            _facebookSDKData = FacebookSdkConfiguration.FacebookSDKData;
            
            base.Load();
        }

        private void SaveToFacebookSettings()
        {
            var labels = FacebookSettings.AppLabels;
            var appIds = FacebookSettings.AppIds;
            var clientTokens = FacebookSettings.ClientTokens;

            if (labels.Count > 0)
            {
                labels[0] = _facebookSDKData.appLabel;
            }
            else
            {
                labels.Add(_facebookSDKData.appLabel);
            }
                
            if (appIds.Count > 0)
            {
                appIds[0] = _facebookSDKData.appID;
            }
            else
            {
                appIds.Add(_facebookSDKData.appID);
            }
                
            if (clientTokens.Count > 0)
            {
                clientTokens[0] = _facebookSDKData.clientToken;
            }
            else
            {
                clientTokens.Add(_facebookSDKData.clientToken);
            }
        }
    }
}
#endif