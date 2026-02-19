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
            
            if(!AppLovinConfiguration.appLovinData.active)
                return;

            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitializedEvent;
            
            MaxSdk.SetHasUserConsent(ConsentData.HasUserConsent);
            
            MaxSdk.SetSdkKey(AppLovinConfiguration.appLovinData.advertisementSdk);
        
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

            bool isInterstitial = AppLovinConfiguration.appLovinData.isUseInterstitial;
            bool isReward = AppLovinConfiguration.appLovinData.isUseReward;

#if UNITY_6000_0_OR_NEWER
            if (isReward)
            {
                RewardedAd.RegisterUnitID(AppLovinConfiguration.appLovinData.androidKeyRew,AppLovinConfiguration.appLovinData.iosKeyRew);
            }
#endif
            if (RuntimePlatform.IPhonePlayer == Application.platform)
            {
                if(isReward) Rewarded.RegisterUnitID(AppLovinConfiguration.appLovinData.iosKeyRew, this);
                if(isInterstitial) InterstitialAd.RegisterUnitID(AppLovinConfiguration.appLovinData.iosKeyInter, this);
                
                Debug.Log("AppLovin iPhone");
            }

            if (RuntimePlatform.Android == Application.platform)
            {
                if(isReward) Rewarded.RegisterUnitID(AppLovinConfiguration.appLovinData.androidKeyRew, this);
                if(isInterstitial) InterstitialAd.RegisterUnitID(AppLovinConfiguration.appLovinData.androidKeyInter, this);
                
                Debug.Log("AppLovin Android");
            }
            
            Debug.Log("AppLovin initialized");
            
            Rewarded.OnSdkInitializedEvent?.Invoke(sdkConfiguration.ToString());
            InterstitialAd.OnSdkInitializedEvent?.Invoke(sdkConfiguration.ToString());
        }
        
    }
}