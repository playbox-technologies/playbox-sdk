using System.Threading.Tasks;
using ConfigManager.Scripts.AppLovin;
using Playbox.Consent;
using UnityEngine;

namespace Playbox.Services
{
    public class AppLovinService : PlayboxService
    {

        public override async void Initialization()
        {
            base.Initialization();

            ServiceType = ServiceType.AppLovin;
            
            if (ConsentData.IsChildUser)
                return;
            
            AppLovinConfiguration.LoadJsonConfig();
            
            if(!AppLovinConfiguration.AppLovinData.active)
                return;

            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitializedEvent;
            
            MaxSdk.SetHasUserConsent(ConsentData.HasUserConsent);
            
            MaxSdk.SetSdkKey(AppLovinConfiguration.AppLovinData.advertisementSdk);

            await WaitSDKInitialized();
        }

        public override void Close()
        {
            base.Close();
            MaxSdkCallbacks.OnSdkInitializedEvent -= OnSdkInitializedEvent;
        }

        private async Task WaitSDKInitialized()
        {
            if (!MaxSdk.IsInitialized())
            {
                MaxSdk.InitializeSdk();
            }
            
            await LLS.PlayerAsyncHelper.WaitUntil(() => MaxSdk.IsInitialized());
            
            ApproveInitialization();
        }

        private void OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {
#if UNITY_6000_0_OR_NEWER
            
            bool isInterstitial = AppLovinConfiguration.AppLovinData.isUseInterstitial;
            bool isReward = AppLovinConfiguration.AppLovinData.isUseReward;

            
            if (isReward)
            {
                RewardedAd.RegisterUnitID(AppLovinConfiguration.AppLovinData.androidKeyRew,AppLovinConfiguration.AppLovinData.iosKeyRew);
                
            }
            if (isInterstitial)
            {
                InterstitialAd.RegisterUnitID(AppLovinConfiguration.AppLovinData.androidKeyInter,AppLovinConfiguration.AppLovinData.iosKeyInter);
            }
            
            Debug.Log("AppLovin initialized");
            return;
#endif
            Debug.LogError("Unity < 6 is not supported!");
            
        }
        
    }
}