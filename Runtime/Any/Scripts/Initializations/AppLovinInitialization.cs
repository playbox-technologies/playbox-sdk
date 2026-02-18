using System.Collections;
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
            
            if(!AppLovinConfiguration.Active)
                return;

            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitializedEvent;
            
            MaxSdk.SetHasUserConsent(ConsentData.HasUserConsent);
            
            MaxSdk.SetSdkKey(AppLovinConfiguration.AdvertisementSdk);
        
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

            bool isInterstitial = AppLovinConfiguration.IsUseInterstitial;
            bool isReward = AppLovinConfiguration.IsUseReward;

#if UNITY_6000
            if (isReward)
            {
                RewardedAd.RegisterUnitID(AppLovinConfiguration.AndroidKey_rew,AppLovinConfiguration.IOSKey_rew);
            }
#endif
            if (RuntimePlatform.IPhonePlayer == Application.platform)
            {
                if(isReward) Rewarded.RegisterUnitID(AppLovinConfiguration.IOSKey_rew, this);
                if(isInterstitial) InterstitialAd.RegisterUnitID(AppLovinConfiguration.IOSKey_inter, this);
                
                Debug.Log("AppLovin iPhone");
            }

            if (RuntimePlatform.Android == Application.platform)
            {
                if(isReward) Rewarded.RegisterUnitID(AppLovinConfiguration.AndroidKey_rew, this);
                if(isInterstitial) InterstitialAd.RegisterUnitID(AppLovinConfiguration.AndroidKey_iter, this);
                
                Debug.Log("AppLovin Android");
            }
            
            Debug.Log("AppLovin initialized");
            
            Rewarded.OnSdkInitializedEvent?.Invoke(sdkConfiguration.ToString());
            InterstitialAd.OnSdkInitializedEvent?.Invoke(sdkConfiguration.ToString());
        }
        
    }
}