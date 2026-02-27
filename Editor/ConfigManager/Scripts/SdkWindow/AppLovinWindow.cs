#if UNITY_EDITOR
using ConfigManager.Scripts.AppLovin;
using Editor.Utils.Layout;
using Playbox.SdkWindow;
using UnityEngine;

namespace Editor.ConfigManager.Scripts.SdkWindow
{
    public class AppLovinWindow : DrawableWindow
    {
        private AppLovinData _appLovinData;
        
        public override void InitName()
        {
            base.InitName();

            if (_appLovinData == null)
            {
                _appLovinData = new AppLovinData();
            }
            
            Name = AppLovinConfiguration.Name;
        }

        public override void HasRenderToggle()
        {
            
        }

        public override void Body()
        {
            if (_appLovinData == null)
            {
                Debug.LogError("appLovinData is null");
                return;
            }
            
            PGUI.SpaceLine();
            
            PGUI.Foldout(ref Active,AppLovinConfiguration.Name,() =>
            {
                PGUI.HorizontalToggle(ref _appLovinData.isAsync, "Is Async");
                
                PGUI.Separator();
                
                PGUI.HorizontalTextField(ref _appLovinData.advertisementSdk,"Advertisement SDK key : ");
                
                PGUI.HorizontalToggle(ref _appLovinData.isUseReward, "Has Use Rewarded Ad : ");

                PGUI.DropdownList(() =>
                {
                    PGUI.HorizontalTextField(ref _appLovinData.iosKeyRew,"IOS rewarded unit id : ");
                    
                    PGUI.Separator();

                    PGUI.HorizontalTextField(ref _appLovinData.androidKeyRew,"Android rewarded unit id : ");
                    
                }, _appLovinData.isUseReward);
                
                PGUI.HorizontalToggle(ref _appLovinData.isUseInterstitial, "Has Use Interstitial Ad : ");
                
                PGUI.DropdownList(() =>
                {
                    PGUI.HorizontalTextField(ref _appLovinData.iosKeyInter,"IOS interstitial unit id : ");
                    PGUI.Separator();
                    PGUI.HorizontalTextField(ref _appLovinData.androidKeyInter,"Android interstitial unit id : ");
                    
                }, _appLovinData.isUseInterstitial);
            });
        }

        public override void Save()
        {
            _appLovinData.active = Active;

            AppLovinConfiguration.AppLovinData = _appLovinData;

            AppLovinConfiguration.SaveJsonConfig();
        }

        public override void Load()
        {
            AppLovinConfiguration.LoadJsonConfig();

            _appLovinData = AppLovinConfiguration.AppLovinData;
            Active = _appLovinData.active;

            base.Load();
        }
    }
}

#endif