using System.Collections;
using ConfigManager.Scripts.AppLovin;
using Playbox.Consent;
using Playbox.SdkConfigurations;
using UnityEngine;

namespace Playbox
{
    public class AppLovinInitialization : PlayboxBehaviour
    {

        public override void Initialization()
        {
            base.Initialization();

            serviceType = ServiceType.AppLovin;
            
            if (ConsentData.IsChildUser)
                return;
            
            AppLovinConfiguration.LoadJsonConfig();
            
            if(!AppLovinConfiguration.AppLovinData.active)
                return;

            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitializedEvent;
            
            MaxSdk.SetHasUserConsent(ConsentData.HasUserConsent);
            
            MaxSdk.SetSdkKey(AppLovinConfiguration.AppLovinData.advertisementSdk);
        
            MaxSdk.InitializeSdk();

            StartCoroutine(initUpd());

        }

        public override void Close()
        {
            base.Close();
            MaxSdkCallbacks.OnSdkInitializedEvent -= OnSdkInitializedEvent;
        }

        private IEnumerator initUpd()
        {
            while (true)
            {
                if (MaxSdk.IsInitialized())
                {
                    ApproveInitialization();
                    yield break;
                }
                else
                {
                    MaxSdk.InitializeSdk();
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private void OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {

            bool isInterstitial = AppLovinConfiguration.AppLovinData.isUseInterstitial;
            bool isReward = AppLovinConfiguration.AppLovinData.isUseReward;

#if UNITY_6000_0_OR_NEWER
            if (isReward)
            {
                RewardedAd.RegisterUnitID(AppLovinConfiguration.AppLovinData.androidKeyRew,AppLovinConfiguration.AppLovinData.iosKeyRew);
            }
#endif
            if (RuntimePlatform.IPhonePlayer == Application.platform)
            {
                if(isReward) Rewarded.RegisterUnitID(AppLovinConfiguration.AppLovinData.iosKeyRew, this);
                if(isInterstitial) InterstitialAd.RegisterUnitID(AppLovinConfiguration.AppLovinData.iosKeyInter, this);
                
                Debug.Log("AppLovin iPhone");
            }

            if (RuntimePlatform.Android == Application.platform)
            {
                if(isReward) Rewarded.RegisterUnitID(AppLovinConfiguration.AppLovinData.androidKeyRew, this);
                if(isInterstitial) InterstitialAd.RegisterUnitID(AppLovinConfiguration.AppLovinData.androidKeyInter, this);
                
                Debug.Log("AppLovin Android");
            }
            
            Debug.Log("AppLovin initialized");
            
            Rewarded.OnSdkInitializedEvent?.Invoke(sdkConfiguration.ToString());
            InterstitialAd.OnSdkInitializedEvent?.Invoke(sdkConfiguration.ToString());
        }
        
    }
}