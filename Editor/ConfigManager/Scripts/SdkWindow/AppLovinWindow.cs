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

            name = AppLovinConfiguration.name;
        }

        public override void Body()
        {
            PGUI.DropdownList(() =>
            {
                PGUI.HorizontalTextField(ref appLovinData.advertisementSdk,"Advertisement SDK key (Only AppLovin Integration Manager) : ");

                PGUI.Separator();
                
                PGUI.HorizontalToggle(ref appLovinData._isUseReward, "Has Use Rewarded Ad : ");
                
                PGUI.Separator();

                PGUI.DropdownList(() =>
                {
                    PGUI.HorizontalTextField(ref appLovinData._iosKeyRew,"IOS rewarded unit id : ");
                    
                    PGUI.Separator();

                    PGUI.HorizontalTextField(ref appLovinData._androidKeyRew,"Android rewarded unit id : ");
                    
                }, appLovinData._isUseReward);

                PGUI.Separator();
                
                PGUI.HorizontalToggle(ref appLovinData._isUseInterstitial, "Has Use Interstitial Ad : ", b=> b.PbInfo());
                
                PGUI.Separator();

                PGUI.DropdownList(() =>
                {
                    PGUI.HorizontalTextField(ref appLovinData._iosKeyInter,"IOS interstitial unit id : ");
                    PGUI.HorizontalTextField(ref appLovinData._androidKeyInter,"Android interstitial unit id : ");
                    
                }, appLovinData._isUseInterstitial);
                
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