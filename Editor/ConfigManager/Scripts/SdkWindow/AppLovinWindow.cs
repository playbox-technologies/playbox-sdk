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

        private bool isUseReward = true;
        private bool isUseInterstitial = true;

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
            advertisementSdk = GUILayout.TextField(advertisementSdk, GUILayout.ExpandWidth(false),
                GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();

            GUILayout.Label("Has Use Rewarded Ad : ", GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight),
                GUILayout.Width(FieldWidth));
            isUseReward = EditorGUILayout.Toggle("", isUseReward, GUILayout.ExpandWidth(false),
                GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            if (isUseReward)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("IOS rewarded unit id : ", GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight),
                    GUILayout.Width(FieldWidth));
                ios_key_rew = GUILayout.TextField(ios_key_rew, GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight),
                    GUILayout.Width(FieldWidth));

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                GUILayout.Label("Android rewarded unit id : ", GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight),
                    GUILayout.Width(FieldWidth));
                android_key_rew = GUILayout.TextField(android_key_rew, GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

                GUILayout.EndHorizontal();
            }

            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();

            GUILayout.Label("Has Use Interstitial Ad : ", GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight),
                GUILayout.Width(FieldWidth));
            isUseInterstitial = EditorGUILayout.Toggle("", isUseInterstitial, GUILayout.ExpandWidth(false),
                GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            if (isUseInterstitial)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("IOS interstitial unit id : ", GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight),
                    GUILayout.Width(FieldWidth));
                ios_key_inter = GUILayout.TextField(ios_key_inter, GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                GUILayout.Label("Android interstitial unit id : ", GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
                android_key_inter = GUILayout.TextField(android_key_inter, GUILayout.ExpandWidth(false),
                    GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

                GUILayout.EndHorizontal();
            }

            hasUnsavedChanges = true;
        }

        public override void Save()
        {
            AppLovinConfiguration.appLovinData._androidKeyRew = android_key_rew;
            AppLovinConfiguration.appLovinData._androidKeyInter = android_key_inter;
            AppLovinConfiguration.appLovinData._iosKeyRew = ios_key_rew;
            AppLovinConfiguration.appLovinData._iosKeyInter = ios_key_inter;
            AppLovinConfiguration.appLovinData.active = active;
            AppLovinConfiguration.appLovinData.advertisementSdk = advertisementSdk;
            AppLovinConfiguration.appLovinData._isUseInterstitial = isUseInterstitial;
            AppLovinConfiguration.appLovinData._isUseReward = isUseReward;

            AppLovinConfiguration.SaveJsonConfig();
        }

        public override void Load()
        {
            AppLovinConfiguration.LoadJsonConfig();

            android_key_rew = AppLovinConfiguration.appLovinData._androidKeyRew;
            android_key_inter = AppLovinConfiguration.appLovinData._androidKeyInter;
            ios_key_rew = AppLovinConfiguration.appLovinData._iosKeyRew;
            ios_key_inter = AppLovinConfiguration.appLovinData._iosKeyInter;
            active = AppLovinConfiguration.appLovinData.active;
            advertisementSdk = AppLovinConfiguration.appLovinData.advertisementSdk;
            isUseReward = AppLovinConfiguration.appLovinData._isUseReward;
            isUseInterstitial = AppLovinConfiguration.appLovinData._isUseInterstitial;

            base.Load();
        }
    }
}

#endif