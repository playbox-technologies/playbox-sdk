using System;
using Playbox.SdkConfigurations;
using UnityEditor;


#if UNITY_EDITOR
using Editor.Utils.Layout;
using UnityEngine;

namespace Playbox.SdkWindow
{
    public class AppsFlyerWindow : DrawableWindow
    {
        private string ios_key = "";
        private string android_key = "";
        private string ios_app_id = "";
        private string android_app_id = "";
        
        private string prev_ios_app_id = "";
        private string prev_android_app_id = "";
    
        private string prev_ios_version;
        private string prev_android_version;
        
        public override void InitName()
        {
            base.InitName();
            
            Name = AppsFlyerConfiguration.Name;
        }

        public override void HasRenderToggle()
        {
            
        }

        public override void Body()
        {
            PGUI.SpaceLine();
            
            PGUI.Foldout(ref Active, AppsFlyerConfiguration.Name, () =>
            {
                PGUI.HorizontalTextField(ref ios_key,"ios sdk key: ");
                PGUI.Separator();
                PGUI.HorizontalTextField(ref ios_app_id,"ios app id : ");
                PGUI.Separator();
                PGUI.HorizontalTextField(ref android_key,"android sdk key: ");
                PGUI.Separator();
                PGUI.HorizontalTextField(ref android_app_id,"android app id : ");
            });
        }

        public override void Save()
        {
            AppsFlyerConfiguration.AndroidKey = ios_key;
            AppsFlyerConfiguration.IOSKey = android_key;
            AppsFlyerConfiguration.Active = Active;
            AppsFlyerConfiguration.IOSAppId = ios_app_id;
            AppsFlyerConfiguration.AndroidAppId = android_app_id;
        
            AppsFlyerConfiguration.SaveJsonConfig();
        }

        public override void Load()
        {
            AppsFlyerConfiguration.LoadJsonConfig();
        
            ios_key = AppsFlyerConfiguration.AndroidKey;
            android_key = AppsFlyerConfiguration.IOSKey;
            Active = AppsFlyerConfiguration.Active;
            ios_app_id = AppsFlyerConfiguration.IOSAppId;
            android_app_id = AppsFlyerConfiguration.AndroidAppId;
        
            base.Load();
        }
    }
}

#endif