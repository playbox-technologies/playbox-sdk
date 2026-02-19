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
            PGUI.Vertical((() =>
            {
                PGUI.Horizontal(() =>
                {
                    PGUI.Label("Advertisement SDK key (Only AppLovin Integration Manager) : ");
                    PGUI.TextField(ref appLovinData.advertisementSdk);
                });

                PGUI.Separator();
                
                PGUI.HorizontalToggle(ref appLovinData._isUseReward, "Has Use Rewarded Ad : ");
                
                PGUI.Separator();

                PGUI.Vertical(() =>
                {
                    PGUI.Horizontal(() =>
                    {
                        PGUI.Label("IOS rewarded unit id : ");
                        PGUI.TextField(ref appLovinData._iosKeyRew);
                    });

                    PGUI.Separator();

                    PGUI.Horizontal(() =>
                    {
                        PGUI.Label("Android rewarded unit id : ");
                        PGUI.TextField(ref appLovinData._androidKeyRew);
                    });
                }, appLovinData._isUseReward);

                EditorGUILayout.Separator();
                
                PGUI.Horizontal(() =>
                {
                    PGUI.Label("Has Use Interstitial Ad : ");
                    PGUI.Toggle(ref appLovinData._isUseInterstitial);
                    
                });
                
                EditorGUILayout.Separator();

                PlayboxLayout.Vertical(() =>
                {
                    PlayboxLayout.Horizontal(() =>
                    {
                        
                        PGUI.Label("IOS interstitial unit id : ");
                        PGUI.TextField(ref appLovinData._iosKeyInter);
                    });

                    PlayboxLayout.Horizontal(() =>
                    {
                        PGUI.Label("Android interstitial unit id : ");
                        PGUI.TextField(ref appLovinData._androidKeyInter);
                        
                    });
                }, appLovinData._isUseInterstitial);
            }), active);
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