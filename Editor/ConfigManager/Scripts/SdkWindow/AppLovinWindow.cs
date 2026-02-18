using System;
using Playbox.SdkConfigurations;
using UnityEditor;

#if UNITY_EDITOR
using UnityEngine;

namespace Playbox.SdkWindow
{
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

            GUILayout.BeginHorizontal();

            GUILayout.Label("Has Use Rewarded Ad : ", GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight),
                GUILayout.Width(FieldWidth));
            appLovinData._isUseReward = EditorGUILayout.Toggle("", appLovinData._isUseReward, GUILayout.ExpandWidth(false),
                GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            if (appLovinData._isUseReward)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("IOS rewarded unit id : ", GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight),
                    GUILayout.Width(FieldWidth));
                appLovinData._iosKeyRew = GUILayout.TextField(appLovinData._iosKeyRew, GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight),
                    GUILayout.Width(FieldWidth));

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                GUILayout.Label("Android rewarded unit id : ", GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight),
                    GUILayout.Width(FieldWidth));
                appLovinData._androidKeyRew = GUILayout.TextField(appLovinData._androidKeyRew, GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

                GUILayout.EndHorizontal();
            }

            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();

            GUILayout.Label("Has Use Interstitial Ad : ", GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight),
                GUILayout.Width(FieldWidth));
            appLovinData._isUseInterstitial = EditorGUILayout.Toggle("", appLovinData._isUseInterstitial, GUILayout.ExpandWidth(false),
                GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            if (appLovinData._isUseInterstitial)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("IOS interstitial unit id : ", GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight),
                    GUILayout.Width(FieldWidth));
                appLovinData._iosKeyInter = GUILayout.TextField(appLovinData._iosKeyInter, GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                GUILayout.Label("Android interstitial unit id : ", GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
                appLovinData._androidKeyInter = GUILayout.TextField(appLovinData._androidKeyInter, GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

                GUILayout.EndHorizontal();
            }

            hasUnsavedChanges = true;
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