using System;
using Playbox.SdkConfigurations;
using UnityEditor;

#if UNITY_EDITOR
using Editor.Utils.Layout;
using UnityEngine;

namespace Playbox.SdkWindow
{
    using PGUI = PlayboxLayout;
    
    public class AppLovinWindow : DrawableWindow
    {
        AppLovinData appLovinData;

        public override void InitName()
        {
            base.InitName();

            name = AppLovinConfiguration.name;
        }

        public override void Body()
        {
            if (!active)
                return;

            GUILayout.BeginHorizontal();

            GUILayout.Label("Advertisement SDK key (Only AppLovin Integration Manager) : ",
                GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            appLovinData.advertisementSdk = GUILayout.TextField(appLovinData.advertisementSdk, GUILayout.ExpandWidth(false),
                GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            
            PGUI.Horizontal(() =>
            {
                PGUI.Label("Has Use Rewarded Ad : ");
                
                PGUI.Toggle("",appLovinData._isUseReward, (b) =>
                {
                    appLovinData._isUseReward = b;
                });
                
            });
            
            PGUI.Separator();
            
            PGUI.Vertical(() =>
            {
                PGUI.Horizontal(() =>
                {
                    PGUI.Label("IOS rewarded unit id : ");
                    PGUI.TextField(appLovinData._iosKeyRew,(text)=> appLovinData._iosKeyRew = text);
                });

                PGUI.Separator();
                
                PGUI.Horizontal(() =>
                {
                    PGUI.Label("Android rewarded unit id : ");
                    PGUI.TextField(appLovinData._androidKeyRew,(text)=> appLovinData._androidKeyRew = text);
                });
                
            },appLovinData._isUseReward);

            EditorGUILayout.Separator();
            
            
            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();

            GUILayout.Label("Has Use Interstitial Ad : ", GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight),
                GUILayout.Width(FieldWidth));
            appLovinData._isUseInterstitial = EditorGUILayout.Toggle("", appLovinData._isUseInterstitial, GUILayout.ExpandWidth(false),
                GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            
            PlayboxLayout.Vertical(() =>
            {
                PlayboxLayout.Horizontal(() =>
                {
                    GUILayout.Label("IOS interstitial unit id : ", GUILayout.ExpandWidth(false),
                        GUILayout.Height(FieldHeight),
                        GUILayout.Width(FieldWidth));
                    appLovinData._iosKeyInter = GUILayout.TextField(appLovinData._iosKeyInter, GUILayout.ExpandWidth(false),
                        GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));    
                });

                PlayboxLayout.Horizontal(() =>
                {
                    GUILayout.Label("Android interstitial unit id : ", GUILayout.ExpandWidth(false),
                        GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
                    
                    appLovinData._androidKeyInter = GUILayout.TextField(appLovinData._androidKeyInter, GUILayout.ExpandWidth(false),
                        GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));    
                    
                });
                
            },appLovinData._isUseInterstitial);
            
        }

        public override void Save()
        {
            appLovinData.active = active;
            
            AppLovinConfiguration.appLovinData = appLovinData;

            AppLovinConfiguration.SaveJsonConfig();
        }

        public override void Load()
        {
            AppLovinConfiguration.LoadJsonConfig();
            
            appLovinData = AppLovinConfiguration.appLovinData;
            active = appLovinData.active;

            base.Load();
        }
    }
}

#endif