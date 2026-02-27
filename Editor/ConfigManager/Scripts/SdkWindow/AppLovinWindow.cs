using ConfigManager.Scripts.AppLovin;
using Playbox.SdkConfigurations;
using Utils.Tools.Extentions;

#if UNITY_EDITOR
using Editor.Utils.Layout;
using UnityEditor;

namespace Playbox.SdkWindow
{
    using PGUI = PlayboxLayout;

    public class AppLovinWindow : DrawableWindow
    {
        AppLovinData appLovinData;

        private int index = 0;

        public override void InitName()
        {
            base.InitName();

            Name = AppLovinConfiguration.Name;
        }

        public override void HasRenderToggle()
        {
            
        }

        public override void Body()
        {
            PGUI.Foldout(ref Active,AppLovinConfiguration.Name,() =>
            {
                EditorGUI.indentLevel++;
                
                PGUI.HorizontalToggle(ref appLovinData.isAsync, "Is Async");
                
                PGUI.Separator();
                
                PGUI.HorizontalTextField(ref appLovinData.advertisementSdk,"Advertisement SDK key : ");

                PGUI.Separator();
                
                PGUI.HorizontalToggle(ref appLovinData.isUseReward, "Has Use Rewarded Ad : ");
                
                PGUI.Separator();

                PGUI.DropdownList(() =>
                {
                    PGUI.HorizontalTextField(ref appLovinData.iosKeyRew,"IOS rewarded unit id : ");
                    
                    PGUI.Separator();

                    PGUI.HorizontalTextField(ref appLovinData.androidKeyRew,"Android rewarded unit id : ");
                    
                }, appLovinData.isUseReward);

                PGUI.Separator();
                
                PGUI.HorizontalToggle(ref appLovinData.isUseInterstitial, "Has Use Interstitial Ad : ");
                
                PGUI.Separator();

                PGUI.DropdownList(() =>
                {
                    PGUI.HorizontalTextField(ref appLovinData.iosKeyInter,"IOS interstitial unit id : ");
                    PGUI.Separator();
                    PGUI.HorizontalTextField(ref appLovinData.androidKeyInter,"Android interstitial unit id : ");
                    
                }, appLovinData.isUseInterstitial);
                
            });
        }

        public override void Save()
        {
            appLovinData.active = Active;

            AppLovinConfiguration.appLovinData = appLovinData;

            AppLovinConfiguration.SaveJsonConfig();
        }

        public override void Load()
        {
            AppLovinConfiguration.LoadJsonConfig();

            appLovinData = AppLovinConfiguration.appLovinData;
            Active = appLovinData.active;

            base.Load();
        }
    }
}

#endif