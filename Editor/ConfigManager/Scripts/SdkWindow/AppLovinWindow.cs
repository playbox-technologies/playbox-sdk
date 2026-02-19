using ConfigManager.Scripts.AppLovin;
using Playbox.SdkConfigurations;
using Utils.Tools.Extentions;

#if UNITY_EDITOR
using Editor.Utils.Layout;

namespace Playbox.SdkWindow
{
    using PGUI = PlayboxLayout;

    public class AppLovinWindow : DrawableWindow
    {
        AppLovinData appLovinData;

        public override void InitName()
        {
            base.InitName();

            name = AppLovinConfiguration.Name;
        }

        public override void Body()
        {
            PGUI.DropdownList(() =>
            {
                PGUI.HorizontalTextField(ref appLovinData.advertisementSdk,"Advertisement SDK key (Only AppLovin Integration Manager) : ");

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
                    PGUI.HorizontalTextField(ref appLovinData.androidKeyInter,"Android interstitial unit id : ");
                    
                }, appLovinData.isUseInterstitial);
                
            }, 
                active);
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