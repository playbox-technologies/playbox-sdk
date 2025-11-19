using System;
using Playbox.SdkConfigurations;
using UnityEditor;

#if UNITY_EDITOR

using UnityEngine;

namespace Playbox.SdkWindow
{
    public class AppLovinWindow : DrawableWindow
    {
        private string ios_key_rew = "";
        private string android_key_rew = "";
        private string ios_key_inter = "";
        private string android_key_inter = "";
        private string advertisementSdk = "";
    
        private string prev_ios_key_rew = "";
        private string prev_android_key_rew = "";
        private string prev_ios_key_inter = "";
        private string prev_android_key_inter = "";
        private string prev_advertisementSdk = "";

        public override void InitName()
        {
            base.InitName();
            
            name = AppLovinConfiguration.Name;
        }

        public override void Body()
        {
            if (!active)
                return;
            
            prev_ios_key_rew = ios_key_rew;
            prev_android_key_rew = android_key_rew;
            prev_ios_key_inter = ios_key_inter;
            prev_android_key_inter = android_key_inter;
            prev_advertisementSdk = advertisementSdk;
            
            GUILayout.BeginHorizontal();
        
            GUILayout.Label("Advertisement SDK key (Only AppLovin Integration Manager) : ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            advertisementSdk = GUILayout.TextField(advertisementSdk, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            
            GUILayout.EndHorizontal();
            
            EditorGUILayout.Separator();
            
            GUILayout.BeginHorizontal();
        
            GUILayout.Label("IOS rewarded unit id : ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            ios_key_rew = GUILayout.TextField(ios_key_rew, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
        
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            
            GUILayout.Label("IOS interstitial unit id : ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            ios_key_inter = GUILayout.TextField(ios_key_inter, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
        
            GUILayout.EndHorizontal();
        
            EditorGUILayout.Separator();
            
            GUILayout.BeginHorizontal();
        
            GUILayout.Label("Android rewarded unit id : ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            android_key_rew = GUILayout.TextField(android_key_rew, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
        
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
        
            GUILayout.Label("Android interstitial unit id : ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            android_key_inter = GUILayout.TextField(android_key_inter, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
        
            GUILayout.EndHorizontal();
        
            hasUnsavedChanges = !(string.Equals(ios_key_rew, prev_ios_key_rew, StringComparison.OrdinalIgnoreCase) && 
                                  string.Equals(ios_key_inter, prev_ios_key_inter, StringComparison.OrdinalIgnoreCase) && 
                                  string.Equals(prev_android_key_rew, android_key_rew, StringComparison.OrdinalIgnoreCase) && 
                                  string.Equals(prev_android_key_inter, android_key_inter, StringComparison.OrdinalIgnoreCase) && 
                                  string.Equals(prev_advertisementSdk, advertisementSdk, StringComparison.OrdinalIgnoreCase));
        
        }

        public override void Save()
        {
            AppLovinConfiguration.AndroidKey_rew = android_key_rew;
            AppLovinConfiguration.AndroidKey_iter = android_key_inter;
            AppLovinConfiguration.IOSKey_rew = ios_key_rew;
            AppLovinConfiguration.IOSKey_inter = ios_key_inter;
            AppLovinConfiguration.Active = active;
            AppLovinConfiguration.AdvertisementSdk = advertisementSdk;
            
            AppLovinConfiguration.SaveJsonConfig();
        }

        public override void Load()
        {
            AppLovinConfiguration.LoadJsonConfig();
        
            android_key_rew = AppLovinConfiguration.AndroidKey_rew;
            android_key_inter = AppLovinConfiguration.AndroidKey_iter;
            ios_key_rew = AppLovinConfiguration.IOSKey_rew;
            ios_key_inter = AppLovinConfiguration.IOSKey_inter;
            active = AppLovinConfiguration.Active;
            advertisementSdk = AppLovinConfiguration.AdvertisementSdk;
        
            base.Load();
        }
    }
}

#endif